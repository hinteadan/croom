using Recognos.Core;
using System;

namespace Croom.Model
{
    public class Comment
    {
        public User By { get; private set; }
        public string Says { get; private set; }
        public DateTime Timestamp { get; private set; }

        private Comment() { }
        public Comment(User user, string comment)
        {
            Check.NotNull(user, "user");
            Check.NotEmpty(comment, "comment");

            this.By = user;
            this.Says = comment;
            this.Timestamp = DateTime.Now;
        }
    }
}
