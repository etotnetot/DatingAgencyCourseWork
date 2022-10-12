using MarriageAgency.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarriageAgency.BLL.Services
{
    public interface IMarriageAgencyService
    {
        public Task<IEnumerable<User>> GetUsers();

        public IEnumerable<UserViewModel> GetUsersViewModels();

        public Task<User> GetUserByName(string nameOfUser);

        public bool SendInvitation(string messageContent, string messageReceiver);

        public Task<IEnumerable<User>> GetBestCandidates(string userLogin);

        public bool AddUser(User userToAdd);

        public bool AddRequirement(Requirement requirementToAdd);
    }
}