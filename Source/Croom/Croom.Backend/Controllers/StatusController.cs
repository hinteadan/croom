using Croom.Backend.Infrastructure;
using Croom.Data;
using Croom.Data.Providers;
using Croom.Engine;
using Croom.Model;
using Recognos.Core;
using System;

namespace Croom.Backend.Controllers
{
    public class StatusController : BaseController
    {
        private readonly ReservationEngine reservationEngine;

        public StatusController(IStoreDataAsKeyValue store)
            : base(store)
        {
            Check.NotNull(store, "store");
            this.reservationEngine =
                new ReservationEngine(() => CurrentUser, new ReservationProvider(store));
            Check.InjectedMembers(this);
        }

        public MeetingRoomStatus Get()
        {
            return reservationEngine.GetCurrentStatus();
        }
    }
}
