using Freelance.Domain.Context;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Freelance.Api.Filters
{
    public class AuthorizeApiAttribute : Attribute, IAuthorizationFilter
    {
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {

                var _context = context.HttpContext.RequestServices.GetRequiredService<ApplicationContext>();

                var getTokenResult = context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues token);
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;
                var expires = tokenS.ValidTo;

                var tokenRecord = _context.UserTokens.FirstOrDefault(x => x.Value == token.FirstOrDefault());

                if (tokenRecord == null)
                {
                    context.Result = new ObjectResult(new ApiResponse<bool>()
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Model = false
                    });
                    return;
                }

                if (DateTime.Now >= expires)
                {
                    context.Result = new ObjectResult(new ApiResponse<bool>()
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Model = false
                    });
                    return;
                }

                if (!string.IsNullOrEmpty(Roles))
                {
                    var tokenClaims = tokenS.Claims.ToList();
                    if (!tokenClaims.Any(x => x.Value == Roles))
                    {
                        context.Result = new ObjectResult(new ApiResponse<bool>()
                        {
                            Status = StatusCodes.Status403Forbidden,
                            Model = false,
                            StatusMessage = "მომხმარებელს არ აქვს საკმარისი უფლება"
                        });
                        return;
                    }
                }
                return;
            }
            catch
            {
                context.Result = new ObjectResult(new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status403Forbidden,
                    Model = false,
                    StatusMessage = "მომხმარებელს არ აქვს საკმარისი უფლება"
                });
                return;
            }
        }
    }
}