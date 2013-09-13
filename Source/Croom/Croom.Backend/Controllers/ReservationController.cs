using Croom.Backend.Infrastructure;
using Croom.Data;
using Croom.Data.Providers;
using Croom.Engine;
using Croom.Model;
using Recognos.Core;
using System;
using System.Collections.Generic;
using Croom.NotificationEngine;
using Croom.Backend.Commands;

namespace Croom.Backend.Controllers
{
    public class ReservationController : BaseController
    {
        private readonly ReservationEngine reservationEngine;

        public ReservationController(IStoreDataAsKeyValue store)
            : base(store)
        {
            Check.NotNull(store, "store");
            this.reservationEngine =
                new ReservationEngine(() => CurrentUser, new ReservationProvider(store));
            Check.InjectedMembers(this);
        }


        public object Post(Reservation reservation)
        {
            Check.NotNull(reservation, "reservation");
            var id = reservationEngine.AddReservation(reservation);
            NotificationEmail.Send(reservation);
            return new { Id = id };
        }

        public object[] Get()
        {
            var result = ReservationProjectionForScheduler.FromReservationCollection(reservationEngine.GetAll());
            return result;
        }

        public Reservation Get(Guid id)
        {
            return reservationEngine.Get(id);
        }

        public void Put(Guid id, Reservation reservation)
        {
            reservationEngine.ChangeReservation(id, reservation);
        }

        public void Delete(Guid id)
        {
            reservationEngine.CancelReservation(id);
        }
    }
}
