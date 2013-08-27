﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Croom.Model;

namespace Croom.Authentication
{
    public interface IAuthenticate
    {
        bool Credentials(string username, string password, out Guid? token);
        bool Authenticate(Guid token, out User authenticatedUser);
        bool Authenticate(Guid token);
        void Invalidate(Guid token);
    }

    public interface IAuthenticateLikeNothing : IAuthenticate { }
}