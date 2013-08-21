using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Croom.Data.Providers;
using Croom.Model;
using Recognos.Core;

namespace Croom.Engine
{
    public class ReservationEngine
    {
        private readonly ReservationProvider repository;
        private readonly User currentUser;

        public ReservationEngine(User currentUser, ReservationProvider reservationProvider)
        {
            Check.NotNull(reservationProvider, "reservationProvider");
            Check.NotNull(currentUser, "currentUser");

            this.repository = reservationProvider;
            this.currentUser = currentUser;
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
            CheckReservation(updatedReservation);

            repository.Save(id, updatedReservation);
        }

        private void CheckReservation(Reservation reservation)
        {
            if (IsOverlappingOthers(reservation))
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

        private bool IsOverlappingOthers(Reservation newReservation)
        {
            return repository.FetchAll().Any(r =>
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
            return reservation.RequestedBy.Username == currentUser.Username;
        }
    }
}
