using Croom.Backend.Infrastructure;
using Croom.Data;
using Croom.Model;

namespace Croom.Backend.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IStoreDataAsKeyValue store) : base(store) { }

        public User Get()
        {
            return CurrentUser;
        }
    }
}
