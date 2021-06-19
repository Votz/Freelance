using Freelance.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Freelance.Services.Helpers
{
    public static class JwtHelper
    {
        public static string Create(string userName, DateTime? expires, IEnumerable<string> roles, TokenOptions options, IEnumerable<ClaimNameValue> claims)
            => Create(
                 expires,
                 options.IssuerSigningKey,
                 ClaimHelper.Create(userName,roles,claims));
        

        private static string Create(DateTime? expires, string key,IEnumerable<Claim> claims)
        {
            var symKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(symKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                expires: expires,
                claims: claims,
                signingCredentials: creds);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
