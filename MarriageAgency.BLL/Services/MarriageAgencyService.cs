using MarriageAgency.BLL.Models;
using System;
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

        public IEnumerable<User> GetBestCandidates(User CurrentUser)
        {
            if (CurrentUser.RequirementID == null)
            {
                throw new Exception("Заполните требования!");
            }

            var usersList = GetUsers();

            foreach (var user in usersList)
            {
                if (user.ClientID != CurrentUser.ClientID && CurrentUser.BestCompability.ToString().ToUpper().Contains(user.ZodiacSign.ToUpper()))
                {
                    user.CompabilityPoint++;
                }
            }

            foreach (var user in usersList)
            {

                if (user.ClientID != CurrentUser.ClientID && CurrentUser.RequirementID.AgeFrom <= user.Age && user.Age <= CurrentUser.RequirementID.AgeTo)
                {
                    user.CompabilityPoint++;
                }

                if (user.ClientID != CurrentUser.ClientID && CurrentUser.RequirementID.Education == user.EducationID)
                {
                    user.CompabilityPoint++;
                }

                if (user.ClientID != CurrentUser.ClientID && CurrentUser.RequirementID.PartnerGender == user.ClientGender)
                {
                    user.CompabilityPoint++;
                }
            }

            var resultList = usersList.Where(u => u.CompabilityPoint > 0 && u.ClientGender == CurrentUser.RequirementID.PartnerGender && u.ClientCouple == null);
            var sortedList = resultList.OrderByDescending(user => user.CompabilityPoint);

            foreach (var user in sortedList)
            {
                user.Age = CalculateAge(user.BirthDate);
            }

            return sortedList;
        }

        public int CalculateAge(DateTime dateOfBirth)
        {
            int currentDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int birthDate = int.Parse(dateOfBirth.ToString("yyyyMMdd"));

            return (currentDate - birthDate) / 10000;
        }
    }
}