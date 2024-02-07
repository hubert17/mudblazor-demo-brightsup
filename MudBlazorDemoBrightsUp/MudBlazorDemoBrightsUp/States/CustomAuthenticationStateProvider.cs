using Microsoft.AspNetCore.Components.Authorization;
using MudBlazorDemoBrightsUp.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace MudBlazorDemoBrightsUp.States
{
    public static class Constants
    {
        public static string JWTToken { get; set; } = "";
    }

    public record CustomUserClaims(string Name = null!, string Email = null!);

    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Constants.JWTToken))
                    return await Task.FromResult(new AuthenticationState(anonymous));
                else
                {
                    var getUserClaims = DecryptToken(Constants.JWTToken);
                    if (getUserClaims == null)
                        return await Task.FromResult(new AuthenticationState(anonymous));
                    else
                    {
                        var claimsPrincipal = SetClaimPrincipal(getUserClaims);
                        return await Task.FromResult(new AuthenticationState(claimsPrincipal));
                    }
                }
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }            
        }

        public static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claims)
        {
            if (string.IsNullOrEmpty(claims.Name))
                return new ClaimsPrincipal();
            else
                return new ClaimsPrincipal(new ClaimsIdentity(
                    new List<Claim>
                    {
                        new(ClaimTypes.Name, claims.Name),
                        new(ClaimTypes.Email, claims.Name)
                    }, "JwtAuth"));
        }

        public async void UpdateAuthenticationState(string jwtToken)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if(!string.IsNullOrEmpty(jwtToken))
            {
                Constants.JWTToken = jwtToken;
                var getUserClaims = DecryptToken(jwtToken);
                claimsPrincipal = SetClaimPrincipal(getUserClaims);
            }
            else
            {
                Constants.JWTToken = null!;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
                return new CustomUserClaims();
            else
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);                

                var name = token.Claims.FirstOrDefault(x => x.Type == "unique_name");
                return new CustomUserClaims(name!.Value, name!.Value);
            }

        }
    }
}
