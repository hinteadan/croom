using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Croom.Data;
using Croom.Data.Providers;
using Croom.Data.Stores;
using Croom.Engine;
using Croom.Model;

namespace Croom.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            User me = new User("hintee", "Hintea Dan", "dan.hintea@recognos.ro");

            ReservationEngine reservationEngine = new ReservationEngine(
                me,
                new ReservationProvider(new InMemoryStore())
                );

            var reservation = new Reservation(me, "Test Reservation", DateTime.Now, DateTime.Now.AddHours(1));
            var otherReservation = new Reservation(me, "Test Reservation", DateTime.Now.AddHours(2), DateTime.Now.AddHours(3));

            reservationEngine.ChangeReservation(reservationEngine.AddReservation(reservation), otherReservation);



            Console.WriteLine("Done @ {0}", DateTime.Now);
            Console.ReadKey();
        }
    }
}
