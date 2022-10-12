using System.Collections.Generic;
using System.Threading.Tasks;
using MarriageAgency.Shared.Models;

namespace MarriageAgency.DAL.Services
{
    public interface IDataService
    {
        public Task<IEnumerable<User>> GetUsers();

        public User GetUserByLogin(string loginOfUser);

        public bool UpdateUser(User userToUpdate);

        public bool DeleteUser(User userToDelete);

        public bool AddUser(User userToAdd);
    }
}