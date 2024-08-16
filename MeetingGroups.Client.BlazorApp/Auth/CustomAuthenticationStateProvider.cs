//using Blazored.SessionStorage;
//using Microsoft.AspNetCore.Components.Authorization;
//using System.Security.Claims;

//namespace MeetingGroups.Client.BlazorApp.Auth;

//// from https://www.linkedin.com/pulse/blazor-server-app-authentication-simplifying-net-core-hilal-yazbek-0mjof/
//public class CustomAuthenticationStateProvider : AuthenticationStateProvider
//{

//    private readonly ISessionStorageService _sessionStorageService;
//    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

//    public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService)
//    {
//        _sessionStorageService = sessionStorageService;
//    }

//    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        var token = await _sessionStorageService.GetItemAsync<string>("token");

//        //if (string.IsNullOrEmpty(token))
//        //{
//            return new AuthenticationState(_anonymous);
//        //}

//        //var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");

//        //var user = new ClaimsPrincipal(identity);

//        //return await Task.FromResult(new AuthenticationState(user));
//    }

//    //public void AuthenticateUser(string token)
//    //{
//    //    var identity = new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwt");

//    //    var user = new ClaimsPrincipal(identity);

//    //    var state = new AuthenticationState(user);

//    //    NotifyAuthenticationStateChanged(Task.FromResult(state));
//    //}
//}
