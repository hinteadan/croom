using Croom.Backend.Infrastructure;
using Croom.Data;
using Croom.Data.Providers;
using Croom.Engine;
using Croom.Model;
using Recognos.Core;
using System;
using System.Collections.Generic;
using Autofac.Integration.WebApi;

namespace Croom.Backend.Controllers
{
    [AutofacControllerConfiguration]
    public class ReservationController : BaseController
    {
        private readonly ReservationEngine reservationEngine;

        public ReservationController(IStoreDataAsKeyValue store)
        {
            Check.NotNull(store, "store");
            this.reservationEngine = new ReservationEngine(CurrentUser, new ReservationProvider(store));
            Check.InjectedMembers(this);
        }

        
        public object Post(Reservation reservation)
        {
            Check.NotNull(reservation, "reservation");
            return new { Id = reservationEngine.AddReservation(reservation) };
        }

        public IEnumerable<KeyValuePair<Guid, Reservation>> Get()
        {
            return reservationEngine.GetAll();
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
