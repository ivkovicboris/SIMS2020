using SIMS2020.Model;
using SIMS2020.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetAll()
        {
            var users = _userRepository.GetAll();
            return users;
        }
        public User Get(long id)
        {
            var user = _userRepository.Get(id);

            return user;
        }
        public User Create(User user)
        {
            var newUser = _userRepository.Create(user);
            return newUser;
        }
        public void Update(User user)
        {
            _userRepository.Update(user);
        }
        public void Delete(User user)
        {
            _userRepository.Delete(user);
        }   
        public User LogIn(User user)
        {

            return _userRepository.LogIn(user);
            
        }
        public bool CheckIfUserNameUnique(string userName)
        {
            if(_userRepository.GetAll().ToList().Find(user => user.UserName.Equals(userName)) == null)
            {
                return true;
            } return false;
        }
        public IEnumerable<User> SearchBy(string searchParam)
        {
            var users = _userRepository.GetAll();
            List<User> result = new List<User>();
            foreach (User user in users)
            {
                if (user.Id.Equals(long.Parse(searchParam))) result.Add(user);
                else if(user.FirstName.ToUpper().Contains(searchParam)) result.Add(user);
                else if (user.LastName.ToUpper().Contains(searchParam)) result.Add(user);
                else if (user.UserName.ToUpper().Contains(searchParam)) result.Add(user);
                else if (user.UserRole.ToString().ToUpper().Contains(searchParam)) result.Add(user);

            }
            return result.Distinct();
           //return _userRepository.LikeSearch<User>(users, "firstName", searchParam);
        }
        
    }
}
