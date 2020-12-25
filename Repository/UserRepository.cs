/***********************************************************************
 * Module:  UserRepository.cs
 * Author:  Boris
 * Purpose: Definition of the Class Repository.UserRepository
 ***********************************************************************/

using SIMS2020.Exception;
using SIMS2020.Model;
using SIMS2020.Repository.Abstract;
using SIMS2020.Repository.CSV;
using SIMS2020.Repository.CSV.Stream;
using SIMS2020.Repository.Sequencer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIMS2020.Repository
{
    public class UserRepository : CSVRepository<User, long>, IUserRepository, IEagerCSVRepository<User, long>

    {
        private const string ENTITY_NAME = "User";

        public UserRepository(ICSVStream<User> stream, ISequencer<long> sequencer)
                                : base(ENTITY_NAME, stream, sequencer)
        {
        }

        public User LogIn(User user)
        {
            var users = GetAll().ToList();
            
                return users.Find(u => u.UserName.Equals(user.UserName) && u.Password.Equals(user.Password));
        }

        public new IEnumerable<User> Find(Func<User, bool> predicate)
            => GetAllEager().Where(predicate);

        public User GetEager(long id) => Get(id);
        public IEnumerable<User> GetAllEager() => GetAll();

    }
}