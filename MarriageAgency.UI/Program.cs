using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using MarriageAgency.UI.Services;
using MarriageAgency.Shared.Models;
using System.Collections.Generic;
using MarriageAgency.UI.Interfaces;

namespace MarriageAgency.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            ConfigureServices(builder.Services);

            await builder.Build().RunAsync();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddAuthorizationCore();

            var serviceProvider = services.BuildServiceProvider();
            services.AddScoped(provider =>
            {
                return new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:5001/api/")
                };
            });

            services
                .AddScoped<IAgencyApiService, AgencyApiService>()
                .AddScoped<AuthenticationStateProvider, TokenAuthenticationStateProvider>()
                .AddScoped<ILocalStorageService, LocalStorageService>();
        }
    }

    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ILocalStorageService _localStorageService { get; set; }

        public TokenAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        private AuthenticationState CreateAnonymous()
        {
            var anonymousIdentity = new ClaimsIdentity();
            var anonymousPrincipal = new ClaimsPrincipal(anonymousIdentity);

            return new AuthenticationState(anonymousPrincipal);
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var currentToken = await _localStorageService.GetAsync<SecurityToken>(nameof(SecurityToken));
            
            if (currentToken == null)
            {
                return CreateAnonymous();
            }
            
            if (String.IsNullOrEmpty(currentToken.AccessToken) || 
                currentToken.ExpiredAt < DateTime.UtcNow)
            {
                return CreateAnonymous();
            }

            var claims = new List<Claim>() 
            {
                new Claim(ClaimTypes.Country, "Russia"),
                new Claim(ClaimTypes.Name, currentToken.UserName),
                new Claim(ClaimTypes.Expired, currentToken.ExpiredAt.ToLongDateString()),
                new Claim(ClaimTypes.Country, "Russia"),
            };

            var currentIdentity = new ClaimsIdentity(claims, "Token");
            var currentPrincipal = new ClaimsPrincipal(currentIdentity);

            return new AuthenticationState(currentPrincipal);
        }

        public void LogOut()
        {
            _localStorageService.RemoveAsync(nameof(SecurityToken));

            var anonymousIdentity = new ClaimsIdentity();
            var anonymousPrincipal = new ClaimsPrincipal(anonymousIdentity);
            var authenticationState = Task.FromResult(new AuthenticationState(anonymousPrincipal));

            NotifyAuthenticationStateChanged(authenticationState);
        }
    }
}