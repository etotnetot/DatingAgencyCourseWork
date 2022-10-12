using MarriageAgency.DAL.Services;
using MarriageAgency.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarriageAgency.BLL.Services
{
    public class MarriageAgencyService : IMarriageAgencyService
    {
        private readonly DataService _dataService;

        public MarriageAgencyService()
        {
            _dataService = new DataService();
        }

        public IEnumerable<UserViewModel> GetUsersViewModels()
        {
            return new List<UserViewModel>()
            {
                new UserViewModel("dima", 4, "argo"),
                new UserViewModel("dim", 4, "argo"),
                new UserViewModel("di", 4, "argo"),
                new UserViewModel("d", 4, "argo")
            };
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _dataService.GetUsers();
        }

        public async Task<User> GetUserByName(string nameOfUser)
        {
            var users = await GetUsers();

            return users.SingleOrDefault(user => user.ClientFullName == nameOfUser);
        }

        public bool SendInvitation(string messageContent, string messageReceiver)
        {
            return true;
        }

        public async Task<IEnumerable<User>> GetBestCandidates(string userLogin)
        {
            var usersList = await GetUsers();
            var CurrentUser = usersList.SingleOrDefault(user => user.ClientFullName == userLogin);

            CurrentUser.BestCompability = _dataService.GetZodiacByName(CurrentUser.ZodiacSign).BestCompability;

            if (CurrentUser.RequirementID == null)
            {
                throw new Exception("Заполните требования!");
            }

            foreach (var user in usersList)
            {
                if (user.ClientID != CurrentUser.ClientID && CurrentUser.BestCompability.ToString().ToUpper().Contains(user.ZodiacSign.ToUpper()))
                    user.CompabilityPoint++;
            }

            foreach (var user in usersList)
            {

                if (user.ClientID != CurrentUser.ClientID && CurrentUser.RequirementID.AgeFrom <= user.Age && user.Age <= CurrentUser.RequirementID.AgeTo)
                    user.CompabilityPoint++;

                if (user.ClientID != CurrentUser.ClientID && CurrentUser.RequirementID.Education == user.EducationID)
                    user.CompabilityPoint++;

                if (user.ClientID != CurrentUser.ClientID && CurrentUser.RequirementID.PartnerGender == user.ClientGender)
                    user.CompabilityPoint++;

            }

            var resultList = usersList.Where(u => u.CompabilityPoint > 0 && u.ClientGender == CurrentUser.RequirementID.PartnerGender && u.ClientCouple == null);
            var sortedList = resultList.OrderByDescending(user => user.CompabilityPoint);

            foreach (var user in sortedList)
                user.Age = CalculateAge(user.BirthDate);

            return sortedList;
        }

        public int CalculateAge(DateTime dateOfBirth)
        {
            int currentDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int birthDate = int.Parse(dateOfBirth.ToString("yyyyMMdd"));

            return (currentDate - birthDate) / 10000;
        }

        public bool AddUser(User userToAdd)
        {
            return _dataService.AddUser(userToAdd);
        }

        public bool AddRequirement(Requirement requirementToAdd)
        {
            return _dataService.AddRequirement(requirementToAdd);
        }
    }
}