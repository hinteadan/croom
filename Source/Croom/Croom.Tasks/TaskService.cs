using Croom.Data;
using Croom.Data.Providers;
using Croom.Engine;
using Croom.Model;
using Croom.Tasks.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Croom.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly IStoreDataAsKeyValue store;

        public void SendNotification(Guid reservationID)
        {
            var currentUser = new User("dan.hintea", "Dan Hintea", "hintea_dan@yahoo.co.uk");
            //var reservationEngine =
                //new ReservationEngine(() => currentUser, new ReservationProvider(store));

            //var reservation = reservationEngine.Get(reservationID);

            var reservation = new Reservation(currentUser, "Daily 4PM With US", new DateTime(2013, 9, 12), new DateTime(2013, 9, 12).AddHours(1));

            var mircea = new User("mircea.nistor", "Mircea Nistor", "mircea.nistor@recognos.ro");
            //var dan = new User("dan.hintea", "Dan Hintea", "dan.hintea@recognos.ro");
            //reservation.AddParticipant(dan);
            reservation.AddParticipant(mircea);
            
            reservation.AddComment(mircea, "Like Nothing");
            reservation.AddComment(mircea, "Be sure to bring food");
            reservation.AddComment(mircea, "Cannot participate");

            NotificationEmail.Send(reservation);
        }
    }
}
