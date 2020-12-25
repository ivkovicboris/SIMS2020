using SIMS2020.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Service
{
    public interface IUserService : IService<User, long>
    {
        User LogIn(User user);
        bool CheckIfUserNameUnique(string userName);
        IEnumerable<User> SearchBy(string searchParam);
    }
}
