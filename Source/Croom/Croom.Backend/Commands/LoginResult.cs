using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recognos.Core;

namespace Croom.Backend.Commands
{
    public struct LoginResult
    {
        public bool IsSuccessful { get; set; }
        public Guid? AuthToken { get; set; }

        public static LoginResult UnSuccessful()
        {
            return new LoginResult
            {
                IsSuccessful = false,
                AuthToken = null
            };
        }

        public static LoginResult Successful(Guid token)
        {
            Check.NotEmpty(token, "token");

            return new LoginResult 
            { 
                IsSuccessful = true,
                AuthToken = token
            };
        }
    }
}