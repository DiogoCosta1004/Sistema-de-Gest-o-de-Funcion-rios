﻿using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClientLibrary.Helpers
{
    public class CustomAuthenticationStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var stringToken = await localStorageService.GetToken();
            if (string.IsNullOrEmpty(stringToken))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var deserializeToken = Serializations.DeserializeJsonString<UserSession>(stringToken);
            if (deserializeToken == null)
                return await Task.FromResult(new AuthenticationState(anonymous));

            var getUserClaims = DecryptToken(deserializeToken.Token);
            if (getUserClaims == null)
                return await Task.FromResult(new AuthenticationState(anonymous));

            var claimsPrincipal = SetClaimPrincipal(getUserClaims);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claims)
        {
            if (claims.Email == null)
                return new ClaimsPrincipal();

            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, claims.Id),
                    new Claim(ClaimTypes.Name, claims.Name),
                    new Claim(ClaimTypes.Email, claims.Email),
                    new Claim(ClaimTypes.Role, claims.Role)
                }, "JwtAuth"));
        }

        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if(userSession.Token != null || userSession.RefreshToken != null)
            {
                var serializeToken = Serializations.SerializeObj(userSession);
                await localStorageService.SetToken(serializeToken);
                var getUserClaims = DecryptToken(userSession.Token);
                claimsPrincipal = SetClaimPrincipal(getUserClaims);
            }
            else
            {
                await localStorageService.RemoveToken();
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
                return new CustomUserClaims();

            var heandler = new JwtSecurityTokenHandler();
            var token = heandler.ReadJwtToken(jwtToken);

            var userId = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier);
            var name = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name);
            var email = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Email);
            var role = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Role);

            return new CustomUserClaims(userId!.Value, name!.Value, email!.Value, role!.Value);
        }
    }
}
