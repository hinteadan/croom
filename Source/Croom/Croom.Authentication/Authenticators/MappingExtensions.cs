using Croom.Model;
using Recognos.Core;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;

namespace Croom.Authentication.Authenticators
{
    internal static class MappingExtensions
    {
        public static User ToUser(this DirectoryEntry directoryEntry, params KeyValuePair<string, string>[] knownEmails)
        {
            Check.NotNull(directoryEntry, "directoryEntry");

            string username = (string)directoryEntry.Properties["sAMAccountName"].Value;

            return new User(
                username,
                (string)directoryEntry.Properties["cn"].Value,
                ComposeEmailAddress(username, knownEmails)
                );
        }

        public static User ToUser(this IIdentity identity, params KeyValuePair<string, string>[] knownEmails)
        {
            Check.NotNull(identity, "identity");
            string username = StripDomainName(identity.Name);
            return new User(
                username,
                username,
                ComposeEmailAddress(identity.Name, knownEmails)
                );
        }

        private static string ComposeEmailAddress(string username, KeyValuePair<string, string>[] knownEmails)
        {
            var knownEmail = knownEmails.SingleOrDefault(e => username.CaseInsensitiveEquals(e.Key));
            if (!knownEmail.Equals(default(KeyValuePair<string, string>)))
            {
                return knownEmail.Value;
            }
            return string.Format("{0}@recognos.ro", username);
        }

        private static string StripDomainName(string fullUserName)
        {
            if (!fullUserName.Contains('\\'))
            {
                return fullUserName;
            }

            return fullUserName.Substring(fullUserName.LastIndexOf('\\') + 1);
        }
    }
}
