using MarriageAgency.BLL.Models;
using System.Collections.Generic;

namespace MarriageAgency.BLL.Services
{
    public interface IMarriageAgencyService
    {
        public IEnumerable<User> GetUsers();

        public User GetUserByName(string nameOfUser);

        public bool SendMessage(string messageContent, string messageReceiver);
    }
}