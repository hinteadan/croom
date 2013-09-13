using Croom.Model;
using Recognos.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Croom.NotificationEngine
{
    public class NotificationEmail
    {
        public static void Send(Reservation res)
        {
            using (SmtpClient client = new SmtpClient())
            {
                var host = ConfigurationManager.AppSettings["Croom.Email.Host"];

                if (!string.IsNullOrWhiteSpace(host))
                {
                    client.Host = host;
                }

                using (var message = CreateMessage(res))
                {
                    client.Send(message);
                }
            }
        }

        private static MailMessage CreateMessage(Reservation res)
        {
            var message = new MailMessage();

            message.From = new MailAddress(ConfigurationManager.AppSettings["Croom.Email.From"]);

            foreach (var participant in res.Participants)
            {
                if (!string.IsNullOrWhiteSpace(participant.Email))
                {
                    message.To.Add(new MailAddress(participant.Email));
                }
            }

            message.Subject = FormatSubject(res);
            message.Body = FormatBody(res);
            message.IsBodyHtml = true;
            //message.Priority

            return message;
        }

        private static string FormatSubject(Reservation res)
        {
            return Format.Invariant("Meeting scheduled for {0} {1}: {2}",
                GetDayName(res.StartsAt), res.StartsAt.ToShortDateString(), res.Title);
        }

        private static string FormatBody(Reservation res)
        {
            string htmlTemplate = string.Empty;
            using (StreamReader sReader = new StreamReader(@"D:\croom\Source\Croom\Croom.Misc\NotificationTemplate.html"))
            {
                htmlTemplate = sReader.ReadToEnd();
                htmlTemplate = htmlTemplate.Replace("[DayName]", GetDayName(res.StartsAt));
                htmlTemplate = htmlTemplate.Replace("[StartDate]", res.StartsAt.ToString());
                htmlTemplate = htmlTemplate.Replace("[ReservationTitle]", res.Title);
                htmlTemplate = htmlTemplate.Replace("[ReservationDescription]", res.Description);
                htmlTemplate = htmlTemplate.Replace("[Participants]", GetParticipants(res));
                htmlTemplate = htmlTemplate.Replace("[Comments]", GetComments(res));
                htmlTemplate = htmlTemplate.Replace("[RequestedBy]", res.RequestedBy.FullName);
                htmlTemplate = htmlTemplate.Replace("[LastsFor]", res.LastsFor.ToString());
            }
            return htmlTemplate;
        }

        private static string GetParticipants(Reservation res)
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine(@"<ul>");
            foreach (var participant in res.Participants)
            {
                b.AppendLine(Format.Invariant(@"<li><b>{0}</b> ({1})</li>",
                    participant.FullName, participant.Email));
            }
            b.AppendLine(@"</ul>");

            return b.ToString();
        }

        private static string GetComments(Reservation res)
        {
            StringBuilder b = new StringBuilder();
            b.AppendLine(@"<ul>");
            foreach (var comment in res.Comments)
            {
                b.AppendLine(Format.Invariant(@"<li><b>{0}</b> - {1}: {2}</li>",
                    comment.By.FullName, comment.Timestamp.ToShortTimeString(), comment.Says));
            }
            b.AppendLine(@"</ul>");

            return b.ToString();
        }

        private static string GetDayName(DateTime date)
        {
            var dayName = string.Empty;
            if (date.Date == DateTime.Today)
            {
                dayName = "Today";
            }
            else if (date.Date == DateTime.Today.AddDays(1))
            {
                dayName = "Tommorow";
            }
            else
            {
                dayName = date.DayOfWeek.ToString();
            }
            return dayName;
        }
    }
}
