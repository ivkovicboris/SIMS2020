using SIMS2020.Model;
using System.Collections.Generic;

namespace SIMS2020.Repository.Abstract
{
    public interface IUserRepository : IRepository<User, long>
    {
        User LogIn(User user);
        //IEnumerable<User> LikeSearch<User>(this IEnumerable<User> data, string key, string searchString);

    }
}
