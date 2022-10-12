﻿using MarriageAgency.Shared.Models;
using MarriageAgency.UI.Interfaces;
using AutoMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace MarriageAgency.UI.Services
{
    public class AgencyApiService : IAgencyApiService
    {
        private readonly HttpClient _httpClient;

        private Mapper mapperInstance;

        /// <summary>
        /// Constructor of a current service.
        /// </summary>
        /// <param name="httpClient">Instance of an HttpClient.</param>
        public AgencyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<User, UserViewModel>()
                    .ForMember(dest => dest.Username, act => act.MapFrom(src => src.ClientFullName))
                    .ForMember(dest => dest.ProfilePicture, act => act.MapFrom(src => src.ProfilePhoto))
                    .ForMember(dest => dest.City, act => act.MapFrom(src => src.ClientCity))
                    .ForMember(dest => dest.Age, act => act.MapFrom(src => src.Age))
            );
            mapperInstance = new Mapper(config);
        }

        /// <summary>
        /// A method for accessing a server in order to get calculated deposit information. 
        /// </summary>
        /// <param name="depositInputModel">An input deposit data.</param>
        /// <returns>A <see cref="Task"> object that represents an asynchronous operation with a result.</returns>
        public async Task<IEnumerable<User>> GetUsers(string queryAction)
        {
            var serverResponse = await _httpClient.GetAsync(@$"MainAgency\{queryAction}");

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new ExternalException($"The response from the server was unsuccessful " +
                    $"due to the following reason: {serverResponse.ReasonPhrase}");
            };

            var apiResponse = serverResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<User>>(apiResponse.Result);
        }

        public async Task<IEnumerable<User>> GetBestCandidates(string userName)
        {
            var inputDataQuery = new Dictionary<string, string>()
            {
                ["userLogin"] = userName
            };

            var uriString = QueryHelpers.AddQueryString(@$"MainAgency\GetBestCandidates", inputDataQuery);

            var serverResponse = await _httpClient.GetAsync(uriString);

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new ExternalException($"The response from the server was unsuccessful " +
                    $"due to the following reason: {serverResponse.ReasonPhrase}");
            };

            var apiResponse = serverResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<User>>(apiResponse.Result);
        }
        
        public async Task<User> GetUserByName(string userName)
        {
            var inputDataQuery = new Dictionary<string, string>()
            {
                ["nameOfUser"] = userName
            };

            var uriString = QueryHelpers.AddQueryString(@$"MainAgency\GetUserByName", inputDataQuery);

            var serverResponse = await _httpClient.GetAsync(uriString);

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new ExternalException($"The response from the server was unsuccessful " +
                    $"due to the following reason: {serverResponse.ReasonPhrase}");
            };

            var apiResponse = serverResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<User>(apiResponse.Result);
        }

        public async Task<bool> AddUser(User userToRegister, Requirement requirementsOfUser)
        {
            var inputDataQuery = new Dictionary<string, string>()
            {
                ["ClientFullName"] = userToRegister.ClientFullName,
                ["ClientID"] = "0",
                ["ClientPassword"] = userToRegister.ClientPassword,
                ["ClientGender"] = userToRegister.ClientGender,
                ["BirthDate"] = userToRegister.BirthDate.ToString("yyyy-MM-dd"),
                ["Email"] = userToRegister.Email,
                ["FetishID"] = userToRegister.FetishID.ToString(),
                ["ClientInformation"] = userToRegister.ClientInformation,
                ["ClientHobbies"] = userToRegister.ClientHobbies,
                ["ClientKids"] = userToRegister.ClientKids,
                ["EducationID"] = userToRegister.EducationID,
                ["BodyType"] = userToRegister.BodyType,
                ["ClientCity"] = userToRegister.ClientCity
            };

            var inputDataQueryRequirements = new Dictionary<string, string>()
            {
                ["RequirementID"] = "0",
                ["AgeFrom"] = requirementsOfUser.AgeFrom.ToString(),
                ["AgeFrom"] = requirementsOfUser.AgeFrom.ToString(),
                ["Education"] = requirementsOfUser.Education,
                ["BodyType"] = requirementsOfUser.BodyType,
                ["PartnerGender"] = requirementsOfUser.PartnerGender,
                ["Kids"] = requirementsOfUser.Kids
            };

            var uriString = QueryHelpers.AddQueryString(@$"MainAgency\AddUser", inputDataQuery);
            var secondUriString = QueryHelpers.AddQueryString(@$"MainAgency\AddRequirement", inputDataQueryRequirements);

            var serverResponse = await _httpClient.GetAsync(uriString);
            var secondServerResponse = await _httpClient.GetAsync(secondUriString);

            if (!serverResponse.IsSuccessStatusCode || !secondServerResponse.IsSuccessStatusCode)
                throw new ExternalException($"The response from the server was unsuccessful " +
                    $"due to the following reason: {serverResponse.ReasonPhrase}");

            return true;
        }

        public List<UserViewModel> MapUsers(IEnumerable<User> currentUsers)
        {
            List<UserViewModel> mappedUsers = new();

            foreach (var user in currentUsers)
            {
                var mappedUser = mapperInstance.Map<UserViewModel>(user);
                mappedUsers.Add(mappedUser);
            }

            return mappedUsers;
        }

        public UserViewModel MapUser(User userToMap)
        {
            return mapperInstance.Map<UserViewModel>(userToMap);
        }
    }
}