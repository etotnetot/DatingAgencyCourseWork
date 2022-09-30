using MarriageAgency.Shared.Models;
using System.Collections.Generic;

namespace MarriageAgency.BLL.Services
{
    public interface IMarriageAgencyService
    {
        public IEnumerable<User> GetUsers();

        public IEnumerable<UserViewModel> GetUsersViewModels();

        public User GetUserByName(string nameOfUser);

        public bool SendInvitation(string messageContent, string messageReceiver);

        public IEnumerable<User> GetBestCandidates(string userLogin);

        public bool AddUser(User userToAdd);

        public bool AddRequirement(Requirement requirementToAdd);
    }
}