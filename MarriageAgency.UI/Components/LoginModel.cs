using MarriageAgency.Shared.Models;
using MarriageAgency.UI.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace MarriageAgency.UI.Components
{
    public class LoginModel : ComponentBase
    {
        [Inject] public ILocalStorageService LocalStorageService { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }

        public LoginViewModel CurrentLoginData { get; set; }

        public LoginModel() 
        { 
            CurrentLoginData = new LoginViewModel(); 
        }

        protected async Task LoginAsync()
        {
            var currentToken = new SecurityToken()
            {
                AccessToken = CurrentLoginData.Password,
                UserName = CurrentLoginData.Username,
                ExpiredAt = DateTime.UtcNow.AddDays(1)
            };

            await LocalStorageService.SetAsync(nameof(SecurityToken), currentToken);
            NavigationManager.NavigateTo("/", true);
        }
    }
}