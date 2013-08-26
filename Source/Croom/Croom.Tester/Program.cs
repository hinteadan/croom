using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Croom.Authentication.Authenticators;
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
            var auth = new ActiveDirectoryAuthenticator(new InMemoryStore());

            Guid? token;
            User user;
            auth.Credentials("dan.hintea", "Lpl4User2012", out token);
            auth.Authenticate(token.Value, out user);

            Console.WriteLine("Done @ {0}", DateTime.Now);
            Console.ReadKey();
        }
    }
}
