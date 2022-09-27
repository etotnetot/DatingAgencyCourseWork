using MarriageAgency.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarriageAgency.UI.Interfaces
{
    public interface IAgencyApiService
    {
        public Task<IEnumerable<User>> GetUsers(string queryAction);

        public List<UserViewModel> MapUsers(IEnumerable<User> currentUsers);

        public Task<User> GetUserByName(string userName);

        public UserViewModel MapUser(User userToMap);
    }
}