using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Recognos.Core;

namespace Croom.Model
{
    public class Reservation
    {
        private readonly Dictionary<string, string> details = new Dictionary<string,string>();
        private readonly List<Comment> comments = new List<Comment>();
        private readonly HashSet<User> particiapnts = new HashSet<User>();


        public User RequestedBy { get; private set; }
        public string Title { get; private set; }
        public ReservationPriority Priority { get; private set; }
        public string Description { get; private set; }
        public DateTime StartsAt { get; private set; }
        public TimeSpan LastsFor { get; private set; }
        public DateTime EndsAt { get; private set; }
        public object Details 
        { 
            get
            {
                JObject json = new JObject();
                foreach(var entry in details)
                {
                    json.Add(entry.Key, JToken.FromObject(entry.Value));
                }
                return json;
            } 
        }
        public IEnumerable<User> Participants
        {
            get
            {
                return particiapnts.ToArray();
            }
        }
        public IEnumerable<Comment> Comments
        {
            get
            {
                return comments.ToArray();
            }
        }

        private Reservation() { }
        public Reservation(User requestedBy, string title, DateTime startsAt, DateTime endsAt) 
        {
            Check.NotNull(requestedBy, "requestedBy");
            Check.NotEmpty(title, "title");
            Check.NotEmpty(startsAt, "startsAt");
            Check.NotEmpty(endsAt, "endsAt");
            Check.Condition(endsAt > startsAt, "Start time must be prior to end time");

            this.RequestedBy = requestedBy;
            this.Title = title;
            this.StartsAt = startsAt;
            this.EndsAt = endsAt;
            this.LastsFor = endsAt - startsAt;
        }

        public void AddDetail(string name, string value)
        {
            Check.NotEmpty(name, "name");
            Check.NotEmpty(value, "value");
            details.Add(name, value);
        }

        public void AddComment(User by, string comment)
        {
            Check.NotNull(by, "by");
            Check.NotEmpty(comment, "comment");
            comments.Add(new Comment(by, comment));
        }

        public void AddParticipant(User participant)
        {
            Check.NotNull(participant, "participant");
            particiapnts.Add(participant);
        }

        public void SetPriority(ReservationPriority priority)
        {
            this.Priority = priority;
        }
    }
}
