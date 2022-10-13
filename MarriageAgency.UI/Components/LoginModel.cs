using MarriageAgency.Shared.Models;
using MarriageAgency.UI.Interfaces;
using MarriageAgency.UI.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace MarriageAgency.UI.Components
{
    public class LoginModel : ComponentBase
    {
        [Inject] 
        public ILocalStorageService LocalStorageService { get; set; }

        [Inject] 
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAgencyApiService AgencyApiService { get; set; }

        public LoginViewModel CurrentLoginData { get; set; }

        public LoginModel() 
        { 
            CurrentLoginData = new LoginViewModel(); 
        }

        protected async Task LoginAsync()
        {
            Task.Delay(10000);
            var currentToken = new SecurityToken()
            {
                AccessToken = CurrentLoginData.Password,
                UserName = CurrentLoginData.Username,
                ExpiredAt = DateTime.UtcNow.AddDays(1)
            };

            var desiredUser = await AgencyApiService.GetUserByName(CurrentLoginData.Username);

            if (CurrentLoginData.Password == desiredUser.ClientPassword)
            /*if (CurrentLoginData.Password == "789")*/
            {
                await LocalStorageService.SetAsync(nameof(SecurityToken), currentToken);
                NavigationManager.NavigateTo(("/user/" + CurrentLoginData.Username), true);
            }  
        }
    }
}