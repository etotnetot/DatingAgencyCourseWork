﻿using MarriageAgency.DAL.Services;
using MarriageAgency.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace MarriageAgency.BLL.Services
{
    public class MarriageAgencyService : IMarriageAgencyService
    {
        public IEnumerable<User> UsersList { get; set; }

        private readonly DataService _dataService;

        private readonly IMemoryCache _memoryCache;

        public MarriageAgencyService(IMemoryCache memoryCache)
        {
            _dataService = new DataService();
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _dataService.GetUsers();
        }

        public IEnumerable<User> GetCachedUsers()
        {
            return _memoryCache.Get<IEnumerable<User>>("users");
        }

        public async Task<User> GetUserByName(string nameOfUser)
        {
            if (_memoryCache.Get<IEnumerable<User>>("users") == null)
            {
                var users = await GetUsers();
                _memoryCache.Set("users", users);
                GetInvitationsForAllUsers();
            }

            return _memoryCache.Get<IEnumerable<User>>("users").SingleOrDefault(user => user.ClientFullName == nameOfUser);
        }

        public async Task<User> GetUserByEmail(string nameOfUser)
        {
            var users = await GetUsers();

            if (_memoryCache.Get<IEnumerable<User>>("users") == null)
            {
                _memoryCache.Set("users", users);
                GetInvitationsForAllUsers();
            }

            return users.SingleOrDefault(user => user.Email == nameOfUser);
        }

        public async Task<User> GetUserById(int idOfUser)
        {
            var users = await GetUsers();

            if (_memoryCache.Get<IEnumerable<User>>("users") == null)
            {
                _memoryCache.Set("users", users);
                GetInvitationsForAllUsers();
            }

            return users.SingleOrDefault(user => user.ClientID == idOfUser);
        }

        public bool SendInvitation(Invitation invitationToSend)
        {
            var usersList = _memoryCache.Get<IEnumerable<User>>("users");

            usersList.ToList().Find(x => x.ClientID == invitationToSend.Sender).SentInvitations.Add(invitationToSend);
            usersList.ToList().Find(x => x.ClientID == invitationToSend.Recipient).MyInvitations.Add(invitationToSend);

            return _dataService.AddInvitation(invitationToSend);
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

        public async Task<bool> AddUser(User userToAdd)
        {
            _dataService.AddUser(userToAdd);

            var users = await GetUsers();
            _memoryCache.Set("users", users);
            GetInvitationsForAllUsers();

            return true;
        }

        public bool AddRequirement(Requirement requirementToAdd)
        {
            return _dataService.AddRequirement(requirementToAdd);
        }

        public async void GetInvitations(string userLogin)
        {
            var usersList = await GetUsers();
            var CurrentUser = usersList.SingleOrDefault(user => user.ClientFullName == userLogin);

            CurrentUser.SentInvitations = _dataService.GetInvitations().Where(i => i.Sender == CurrentUser.ClientID).ToList();
            CurrentUser.MyInvitations = _dataService.GetInvitations().Where(i => i.Recipient == CurrentUser.ClientID).ToList();
        }

        public bool GetInvitationsForAllUsers()
        {
            var usersList = _memoryCache.Get<IEnumerable<User>>("users").ToList();

            /*foreach (var item in usersList)
            {
                item.SentInvitations = _dataService.GetInvitations().Where(i => i.Sender == item.ClientID).ToList();
                item.MyInvitations = _dataService.GetInvitations().Where(i => i.Recipient == item.ClientID).ToList();
            }*/

            usersList.ForEach(item =>
            {
                item.SentInvitations = _dataService.GetInvitations().Where(i => i.Sender == item.ClientID).ToList();
                item.MyInvitations = _dataService.GetInvitations().Where(i => i.Recipient == item.ClientID).ToList();
            });

            return true;
        }
    }
}