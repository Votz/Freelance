using Freelance.Domain.Context;
using Freelance.Domain.Entities;
using Freelance.Services.Helpers;
using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;

        public AuthorizationService(IConfiguration configuration, IHttpContextAccessor httpContext, UserManager<User> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _httpContext = httpContext;
        }

        public async Task<ApiResponse<LoginResponseModel>> LogIn(LoginRequestModel model)
        {
            try
            {

            
            var result = new ApiResponse<LoginResponseModel>();

            var user = _context.Users.Where(x => x.Email == model.Email).FirstOrDefault();

            if (user == null)
            {
                result.Status = StatusCodes.Status400BadRequest;
                return result;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var tokenOptions = new Shared.Models.TokenOptions()
            {
                ExpireMinutes = int.Parse(_configuration.GetSection("TokenOptions").GetSection("ExpireMinutes").Value),
                IssuerSigningKey = _configuration.GetSection("TokenOptions").GetSection("IssuerSigningKey").Value,
            };

            var expires = DateTime.Now.AddMinutes(tokenOptions.ExpireMinutes);
            result.Model = new LoginResponseModel
            {
                ExpiryDate = expires,
                TokenType = "Bearer"
            };

            var claims = new List<ClaimNameValue>();
            if (userRoles.Count() > 0)
            {
                claims.Add(
                    new ClaimNameValue()
                    {
                        Key = "UserId",
                        Value = user.Id
                    });

                foreach (var role in userRoles)
                {
                    claims.Add(
                        new ClaimNameValue()
                        {
                            Key = "UserRole",
                            Value = role
                        });
                }
            }

            result.Model.Token = JwtHelper.Create(null, expires, userRoles, tokenOptions, claims);

            if (!string.IsNullOrEmpty(result.Model.Token))
            {
                var userTokens = _context.UserTokens.ToList();
                if (!userTokens.Any(x => x.UserId == user.Id && x.Name == user.UserName))
                {

                    _context.UserTokens.Add(new IdentityUserToken<string>()
                    {
                        UserId = user.Id,
                        Name = user.UserName,
                        LoginProvider = "HireHaralo",
                        Value = result.Model.Token
                    });
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var userTokenRecord = userTokens.FirstOrDefault(x => x.UserId == user.Id && x.Name == user.UserName);
                    _context.Remove(userTokenRecord);
                    _context.UserTokens.Add(new IdentityUserToken<string>()
                    {
                        UserId = user.Id,
                        Name = user.UserName,
                        LoginProvider = "HireHaralo",
                        Value = result.Model.Token
                    });
                    _context.SaveChanges();
                }
                return result;
            }
            result.Status = StatusCodes.Status400BadRequest;
            return result;
            }
            catch
            {
                var result = new ApiResponse<LoginResponseModel>();
                result.Status = StatusCodes.Status400BadRequest;
                return result;

            }
        }

        public async Task<ApiResponse<bool>> Logout()
        {
            var userToken = GetUserToken();

            if (string.IsNullOrEmpty(userToken))
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status403Forbidden,
                    StatusMessage = "არაავტორიზირებული მოთხოვნა",
                    Model = false
                };
            }
            var tokenRecord = await _context.UserTokens.FirstOrDefaultAsync(x => x.Value == userToken);
            if (tokenRecord == null)
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Model = false
                };
            }
            _context.UserTokens.Remove(tokenRecord);
            await _context.SaveChangesAsync();
            return new ApiResponse<bool>()
            {
                Status = StatusCodes.Status200OK,
                Model = true
            };
        }

        public async Task<ApiResponse<CheckAuthorityResponseModel>> CheckAuthority()
        {
            var userToken = GetUserToken();

            if (string.IsNullOrEmpty(userToken))
            {
                return new ApiResponse<CheckAuthorityResponseModel>()
                {
                    Status = StatusCodes.Status403Forbidden,
                    Model = new CheckAuthorityResponseModel()
                    {
                        IsAuthorized = false,
                    }
                };
            }
            else
            {
                var tokenRecord = await _context.UserTokens.FirstOrDefaultAsync(x => x.Value == userToken);

                if (tokenRecord == null)
                {
                    return new ApiResponse<CheckAuthorityResponseModel>()
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Model = new CheckAuthorityResponseModel()
                        {
                            IsAuthorized = false,
                        }
                    };
                }

                var userTokenValidity = GetUserTokenValidity();

                if (!userTokenValidity.IsValid)
                {
                    return new ApiResponse<CheckAuthorityResponseModel>()
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Model = new CheckAuthorityResponseModel()
                        {
                            IsAuthorized = false,
                        }
                    };
                }
                if (userTokenValidity.IsValid && userTokenValidity.ValidTo > DateTime.Now)
                {
                    var userId = GetUserId();
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                    return new ApiResponse<CheckAuthorityResponseModel>()
                    {
                        Status = StatusCodes.Status200OK,
                        Model = new CheckAuthorityResponseModel()
                        {
                            IsAuthorized = true,
                            Username = user.UserName
                        }
                    };
                }
            }
            return new ApiResponse<CheckAuthorityResponseModel>()
            {
                Status = StatusCodes.Status403Forbidden,
                Model = new CheckAuthorityResponseModel()
                {
                    IsAuthorized = false,
                }
            };
        }

        public string GetUserId()
        {
            var getAuthorizationToken = _httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues token);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var jwtUserToken = jsonToken as JwtSecurityToken;

            var tokenClaims = jwtUserToken.Claims.ToList();

            if (tokenClaims.Any(x => x.Value == "UserId"))
            {
                return tokenClaims.FirstOrDefault(x => x.Value == "UserId").ToString();
            }
            return null;
        }

        public string GetUserToken()
        {
            var getAuthorizationToken = _httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues token);

            if (string.IsNullOrEmpty(token))
                return null;
            return token;
        }

        public GetUserTokenValidityModel GetUserTokenValidity()
        {
            var getAuthorizationToken = _httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues token);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var jwtUserToken = jsonToken as JwtSecurityToken;

            var expires = jwtUserToken.ValidTo;
            if (DateTime.Now >= expires)
            {
                return new GetUserTokenValidityModel();
            }
            return new GetUserTokenValidityModel(true, expires);
        }
    }
}
