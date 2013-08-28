using Croom.Data.Providers;
using Croom.Model;
using Recognos.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Croom.Engine
{
    public class ReservationEngine
    {
        private readonly ReservationProvider repository;
        private readonly Lazy<User> currentUser;

        public ReservationEngine(Func<User> currentUserProvider, ReservationProvider reservationProvider)
        {
            Check.NotNull(reservationProvider, "reservationProvider");
            Check.NotNull(currentUserProvider, "currentUserProvider");

            this.repository = reservationProvider;
            this.currentUser = new Lazy<User>(currentUserProvider);
        }

        public Guid AddReservation(Reservation newReservation)
        {
            CheckReservation(newReservation);

            return repository.Save(newReservation);
        }

        public void CancelReservation(Guid id)
        {
            Check.NotEmpty(id, "reservationId");
            CheckOwnership(id);

            repository.Remove(id);
        }

        public void ChangeReservation(Guid id, Reservation updatedReservation)
        {
            CheckOwnership(id);
            CheckReservation(updatedReservation, id);

            repository.Save(id, updatedReservation);
        }

        public IEnumerable<KeyValuePair<Guid, Reservation>> GetAll()
        {
            return repository.FetchAll();
        }

        public Reservation Get(Guid id)
        {
            return repository.Load(id);
        }

        private void CheckReservation(Reservation reservation, params Guid[] ignore)
        {
            if (IsOverlappingOthers(reservation, ignore))
            {
                throw new InvalidOperationException("The reservation is overlapping some existing reservations");
            }
        }

        private void CheckOwnership(Guid reservationId)
        {
            var reservation = repository.Load(reservationId);
            Check.Condition(reservation != null, "Cannot find a reservation with given reservationId, {0}", reservationId);
            Check.Condition(IsCurrentUserReservationOwner(reservation), "This reservation was requested by another user");
        }

        private bool IsOverlappingOthers(Reservation newReservation, params Guid[] ignore)
        {
            return repository.FetchAll()
                .Where(r => !ignore.Contains(r.Key))
                .Any(r =>
                IsBetweenInclusive(newReservation.StartsAt, r.Value.StartsAt, r.Value.EndsAt)
                || IsBetweenInclusive(newReservation.EndsAt, r.Value.StartsAt, r.Value.EndsAt)
                );
        }

        private bool IsBetweenInclusive(DateTime dateToCheck, DateTime min, DateTime max)
        {
            return dateToCheck >= min && dateToCheck <= max;
        }

        private bool IsCurrentUserReservationOwner(Reservation reservation)
        {
            Check.Condition(currentUser.Value != null, "Current user is undefined");
            return reservation.RequestedBy.Username == currentUser.Value.Username;
        }

        public MeetingRoomStatus GetCurrentStatus()
        {
            //TODO: Implement This

            //return new MeetingRoomStatus
            //{
            //    IsOccupied = true,
            //    CurrentReservation = new Reservation(new User("vlad.sda", "mama leone", "asdasd@sdas.com"), "Dummy Title", DateTime.Now, DateTime.Now.AddHours(2))
            //};
            return new MeetingRoomStatus
            {
                IsOccupied = false,
                CurrentReservation = null
            };
        }
    }
}
