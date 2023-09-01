using MarriageAgency.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarriageAgency.BLL.Services
{
    public interface IMarriageAgencyService
    {
        public Task<IEnumerable<User>> GetUsers();

        public Task<User> GetUserByName(string nameOfUser);

        public Task<User> GetUserByEmail(string nameOfUser);

        public Task<User> GetUserById(int idOfUser);

        public bool SendInvitation(Invitation invitationToSend);

        public Task<IEnumerable<User>> GetBestCandidates(string userLogin);

        public IEnumerable<User> GetCachedUsers();

        public Task<bool> AddUser(User userToAdd);

        public bool AddRequirement(Requirement requirementToAdd);

        public bool GetInvitationsForAllUsers();
    }
}