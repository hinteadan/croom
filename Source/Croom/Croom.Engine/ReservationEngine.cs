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
            if (IsOverlappingOthers(newReservation))
            {
                throw new InvalidOperationException("The reservation is overlapping some existing reservations");
            }

            return repository.Save(newReservation);
        }

        public void CancelReservation(Guid id)
        {
            Check.NotEmpty(id, "id");
            var reservation = repository.Load(id);
            Check.Condition(reservation != null , "Cannot find a reservation with given id, {0}", id);
            Check.Condition(IsCurrentUserReservationOwner(reservation), "This reservation was requested by another user");

            repository.Remove(id);
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
