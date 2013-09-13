using Croom.Model;
using Croom.NotificationEngine;
using Recognos.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Croom.Tasks
{
    public class TaskWService : ServiceBase
    {
        private readonly SimpleScheduler schedule;
        public ServiceHost serviceHost = null;
        public TaskWService()
        {
            ServiceName = "TaskService";
            
            this.schedule = new SimpleScheduler(() => CheckForReservations(), DateTime.Now.AddSeconds(5).ToString("hh:mm:ss"), new TimeSpan(0,5,0));
        }

        public static void Main()
        {
            ServiceBase.Run(new TaskWService());
        }

        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            if (this.schedule != null)
            {
                this.schedule.Start();
            }
        }

        private static void CheckForReservations()
        {
            var user = new User("mircea.nistor", "Mircea Nistor", "mircea.nistor@recognos.ro");

            var reservation = new Reservation(user, "Daily LPL Meeting", new DateTime(2013, 9, 13), new DateTime(2013, 9, 13).AddHours(1));
            reservation.AddParticipant(user);
            NotificationEmail.Send(reservation);
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }

        private void InitializeComponent()
        {
            // 
            // TaskWService
            // 
            this.ServiceName = "CroomTaskService";

        }
    }


}
