using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recognos.Core;

namespace Croom.Model
{
    public class User
    {
        public string Username { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }

        private User() { }
        public User(string username, string fullName, string email)
        {
            Check.NotEmpty(username, "username");
            Check.NotEmpty(fullName, "fullName");
            Check.ValidEmail(email, "email");

            this.Username = username;
            this.FullName = fullName;
            this.Email = email;
        }
    }
}
