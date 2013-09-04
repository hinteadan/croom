using Croom.Backend.Commands;
using Croom.Backend.Infrastructure;
using Croom.Data;

namespace Croom.Backend.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IStoreDataAsKeyValue store) : base(store) { }

        public UserCheckResult Get()
        {
            return CurrentUser != null ?
                new UserCheckResult
                {
                    IsUserLoggedIn = true,
                    FullName = CurrentUser.FullName
                }
                : new UserCheckResult { IsUserLoggedIn = false };
        }
    }
}
