using MarriageAgency.Shared.Models;
using MarriageAgency.UI.Interfaces;
using AutoMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Linq;

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

            //txtBlock.Text = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<User>>(await apiResponse);
        }

        public async Task<List<UserViewModel>> GetUsersMapped(string queryAction)
        {
            var serverResponse = await _httpClient.GetAsync(@$"MainAgency\{queryAction}");

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new ExternalException($"The response from the server was unsuccessful " +
                    $"due to the following reason: {serverResponse.ReasonPhrase}");
            };

            var apiResponse = serverResponse.Content.ReadAsStringAsync();

            return MapUsers(JsonConvert.DeserializeObject<IEnumerable<User>>(apiResponse.Result));
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

        public async Task<User> GetUserByEmail(string userName)
        {
            var inputDataQuery = new Dictionary<string, string>()
            {
                ["nameOfUser"] = userName
            };

            var uriString = QueryHelpers.AddQueryString(@$"MainAgency\GetUserByEmail", inputDataQuery);

            var serverResponse = await _httpClient.GetAsync(uriString);

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new ExternalException($"The response from the server was unsuccessful " +
                    $"due to the following reason: {serverResponse.ReasonPhrase}");
            };

            var apiResponse = serverResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<User>(apiResponse.Result);
        }

        public async Task<User> GetUserById(int id)
        {
            var inputDataQuery = new Dictionary<string, string>()
            {
                ["idOfUser"] = id.ToString()
            };

            var uriString = QueryHelpers.AddQueryString(@$"MainAgency\GetUserById", inputDataQuery);

            var serverResponse = await _httpClient.GetAsync(uriString);

            if (!serverResponse.IsSuccessStatusCode)
            {
                throw new ExternalException($"The response from the server was unsuccessful " +
                    $"due to the following reason: {serverResponse.ReasonPhrase}");
            };

            var apiResponse = serverResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<User>(apiResponse.Result);
        }

        public async Task<bool> AddUser(UserInputModel userInputModel)
        {
            var stringContentUser = new StringContent(System.Text.Json.JsonSerializer.Serialize(userInputModel.User),
                Encoding.UTF8,
                "application/json");
            var stringContentReq = new StringContent(System.Text.Json.JsonSerializer.Serialize(userInputModel.RequirementOfUser),
                Encoding.UTF8,
                "application/json");

            var secondServerResponse = await _httpClient.PostAsync(@$"MainAgency\AddRequirement", stringContentReq);
            var serverResponse = await _httpClient.PostAsync(@$"MainAgency\AddUser", stringContentUser);

            /*var serverResponse = await _httpClient.GetAsync(uriString);
            var secondServerResponse = await _httpClient.GetAsync(secondUriString);*/

            if (!serverResponse.IsSuccessStatusCode || !secondServerResponse.IsSuccessStatusCode)
                throw new ExternalException($"The response from the server was unsuccessful " +
                    $"due to the following reason: {serverResponse.ReasonPhrase}");

            return true;
        }

        public async Task<bool> SendInvitation(Invitation invitationToSend)
        {
            var stringContentInvitation = new StringContent(System.Text.Json.JsonSerializer.Serialize(invitationToSend),
                Encoding.UTF8,
                "application/json");

            var serverResponse = await _httpClient.PostAsync(@$"MainAgency\SendInvitation", stringContentInvitation);

            if (!serverResponse.IsSuccessStatusCode)
                throw new ExternalException($"The response from the server was unsuccessful " +
                    $"due to the following reason: {serverResponse.ReasonPhrase}");

            return true;
        }
       

        public List<UserViewModel> MapUsers(IEnumerable<User> currentUsers)
        {
            List<UserViewModel> mappedUsers = new();

            currentUsers.ToList().ForEach(item =>
            {
                var mappedUser = mapperInstance.Map<UserViewModel>(item);
                mappedUsers.Add(mappedUser);
            });

            return mappedUsers;
        }

        public UserViewModel MapUser(User userToMap)
        {
            return mapperInstance.Map<UserViewModel>(userToMap);
        }
    }
}