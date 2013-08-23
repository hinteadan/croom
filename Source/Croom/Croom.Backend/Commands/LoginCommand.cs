using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Croom.Backend.Commands
{
    public struct LoginCommand
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}