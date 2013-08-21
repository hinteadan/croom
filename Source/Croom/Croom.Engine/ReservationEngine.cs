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

        public ReservationEngine(ReservationProvider reservationProvider)
        {
            Check.NotNull(reservationProvider, "reservationProvider");

            this.repository = reservationProvider;
        }

        public Guid AddReservation(Reservation newReservation)
        {
            if (IsOverlappingOthers(newReservation))
            {
                throw new InvalidOperationException("The reservation is overlapping some existing reservations");
            }

            return repository.Save(newReservation);
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
    }
}
