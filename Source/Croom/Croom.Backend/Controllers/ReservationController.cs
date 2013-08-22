using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Croom.Backend.Infrastructure;
using Croom.Data;
using Croom.Data.Providers;
using Croom.Engine;
using Croom.Model;
using Recognos.Core;

namespace Croom.Backend.Controllers
{
    public class ReservationController : BaseController
    {
        private readonly ReservationEngine reservationEngine;

        public ReservationController(IStoreDataAsKeyValue store)
        {
            Check.NotNull(store, "store");
            reservationEngine = new ReservationEngine(CurrentUser, new ReservationProvider(store));
            Check.InjectedMembers(this);
        }

        public object Post(Reservation reservation)
        {
            return new { Id = reservationEngine.AddReservation(reservation) };
        }
    }
}
