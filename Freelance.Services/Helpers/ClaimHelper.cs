using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Freelance.Services.Helpers
{
    public static class ClaimHelper
    {
        public static List<Claim> Create(
            string userName = null,
            IEnumerable<string> roles = null,
            IEnumerable<ClaimNameValue> claimCollection = null)
        {
            var claims = new List<Claim>();
            if (!string.IsNullOrEmpty(userName))
            {
                claims.Add(new Claim(CustomClaimTypes.UserName, userName));
            }

            if (roles != null)
                claims.AddRange(roles.Select(role => new Claim(CustomClaimTypes.Role, role)));

            if (claimCollection != null)
            {
                claims.AddRange(claimCollection.Select(c => new Claim(c.Key, c.Value)));
            }
            return claims;
        }
    }
}
