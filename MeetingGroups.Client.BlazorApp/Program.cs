using MeetingGroups.Client.BlazorApp.Auth;
using MeetingGroups.Client.BlazorApp.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var authServerSettings = new AuthServerSettings();
builder.Configuration.GetRequiredSection(nameof(AuthServerSettings))
    .Bind(authServerSettings);
builder.Services.ConfigureAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme, MS_OIDC_SCHEME, authServerSettings);

builder.Services.AddOptions<OpenIdConnectOptions>(MS_OIDC_SCHEME)
    .Configure(oidcOptions =>
    {
        // MINE, additional

        // RequireHttpsMetadata is false in Development else Keyclock throws an error.
        // This should be true in Production
        oidcOptions.RequireHttpsMetadata = authServerSettings.RequireHttpsMetadata;

        oidcOptions.Scope.Add("MeetingsModuleWebApi_ClientScope");

        oidcOptions.Events.OnTokenValidated += eventArgs =>
        {
            // See https://codyanhorn.tech/blog/blazor/2020/09/06/Blazor-Server-Get-Access-Token-for-User.html
            var accessToken = eventArgs.TokenEndpointResponse.AccessToken;
            eventArgs.Principal.AddIdentity(new ClaimsIdentity(
                [
                    new Claim("access_token", accessToken)
                ]
            ));

            return Task.CompletedTask;
        };
    });

// ConfigureCookieOidcRefresh attaches a cookie OnValidatePrincipal callback to get
// a new access token when the current one expires, and reissue a cookie with the
// new access token saved inside. If the refresh fails, the user will be signed
// out. OIDC connect options are set for saving tokens and the offline access
// scope.
builder.Services.ConfigureCookieOidcRefresh(CookieAuthenticationDefaults.AuthenticationScheme, MS_OIDC_SCHEME);

builder.Services.AddAuthorization();

builder.Services.AddHttpClient();

//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGroup("/authentication").MapLoginAndLogout();

app.Run();

public partial class Program
{
    public const string MS_OIDC_SCHEME = "MicrosoftOidc";
}
