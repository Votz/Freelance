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
        public static string Create(string userName, DateTime? expires, IEnumerable<string> roles, TokenOptions options)
            => Create(
                 expires,
                 options.IssuerSigningKey);
        
        private static string Create(DateTime? expires, string key)
        {
            var symKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(symKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                expires: expires,
                signingCredentials: creds);
            var jwtToken = "Bearer " + new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
