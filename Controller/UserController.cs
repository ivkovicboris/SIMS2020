using SIMS2020.Model;
using SIMS2020.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Controller
{
    public class UserController 
    {
        private readonly IUserService _userService;

        
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public IEnumerable<User> GetAll() => _userService.GetAll();
        public User Get(long id) => _userService.Get(id);
        public User Create(User user) => _userService.Create(user);
        public void Update(User user) => _userService.Update(user);
        public void Delete(User user) => _userService.Delete(user);
        public User LogIn(User userLogIn)
        {
            return _userService.LogIn(userLogIn);
        }
        public bool CheckIfUserNameUnique(string userName)
        {
            return _userService.CheckIfUserNameUnique(userName);
        }

        public IEnumerable<User> SearchUsers(string searchParam)
        {
            return _userService.SearchBy(searchParam.ToUpper());
        }
    }
}
