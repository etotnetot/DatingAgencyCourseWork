using MarriageAgency.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarriageAgency.UI.Interfaces
{
    public interface IAgencyApiService
    {
        public Task<IEnumerable<User>> GetUsers(string queryAction);

        public Task<List<UserViewModel>> GetUsersMapped(string queryAction);

        public List<UserViewModel> MapUsers(IEnumerable<User> currentUsers);

        public Task<IEnumerable<User>> GetBestCandidates(string userName);

        public Task<User> GetUserByName(string userName);

        public Task<User> GetUserByEmail(string userName);

        public Task<User> GetUserById(int id);

        public UserViewModel MapUser(User userToMap);

        public Task<bool> AddUser(UserInputModel userInputModel);

        public Task<bool> SendInvitation(Invitation invitationToSend);
    }
}