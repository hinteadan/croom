using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Croom.Model;

namespace Croom.Backend.Commands
{
    public class ReservationProjectionForScheduler
    {
        public static object[] FromReservationCollection(IEnumerable<KeyValuePair<Guid, Reservation>> reservations)
        {
            return reservations.Select(r => new {
                start_date = r.Value.StartsAt.ToString("yyyy-M-dd HH:mm"),
                end_date = r.Value.EndsAt.ToString("yyyy-M-dd HH:mm"),
                text = r.Value.Title
            })
            .ToArray();
        }
    }
}