using SIMS2020.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS2020.Repository.CSV.Converter
{
    public class UserCSVConverter : ICSVConverter<User>
    {
        private readonly string _delimiter;
        
        public UserCSVConverter(string delimiter)
        {
            _delimiter = delimiter;
        }

        public string ConvertEntityToCSVFormat(User user)
           => string.Join(_delimiter,
               user.Id,
               user.FirstName,
               user.LastName,
               user.UserName,
               user.Password,
               user.UserRole.ToString()
               );

        public User ConvertCSVFormatToEntity(string userCSVFormat)
        {
            string[] tokens = userCSVFormat.Split(_delimiter.ToCharArray());
           
            return new User(
                long.Parse(tokens[0]),
                tokens[1],
                tokens[2],
                tokens[3],
                tokens[4],
                (UserRole)Enum.Parse(typeof(UserRole),tokens[5]));
        }
    }
}
