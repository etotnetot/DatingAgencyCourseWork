using MarriageAgency.BLL.Models;
using System.Collections.Generic;
using System.Linq;

namespace MarriageAgency.BLL.Services
{
    class MarriageAgencyService : IMarriageAgencyService
    {
        public IEnumerable<User> GetUsers()
        {
            return new List<User>();
        }

        public User GetUserByName(string nameOfUser)
        {
            return GetUsers().SingleOrDefault(user => user.FullName == nameOfUser);
        }

        public bool SendMessage(string messageContent, string messageReceiver)
        {
            return true;
        }
    }
}