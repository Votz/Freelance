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
                    result.StatusMessage = "არასწორი იმეილი ან პაროლი";
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
                claims.Add(
                        new ClaimNameValue()
                        {
                            Key = "UserId",
                            Value = user.Id
                        });


                if (userRoles.Count() > 0)
                {
                    
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
                    result.Status = StatusCodes.Status200OK;
                    return result;
                }
                result.Status = StatusCodes.Status400BadRequest;
                result.StatusMessage = "არასწორი იმეილი ან პაროლი";
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
            try
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
            catch
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<ApiResponse<bool>> CheckAuthority()
        {
            try
            {

            
            var userToken = GetUserToken();

            if (string.IsNullOrEmpty(userToken))
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status403Forbidden,
                    Model = false
                };
            }
            else
            {
                var tokenRecord = await _context.UserTokens.FirstOrDefaultAsync(x => x.Value == userToken);

                if (tokenRecord == null)
                {
                    return new ApiResponse<bool>()
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Model = false
                    };
                }

                var userTokenValidity = GetUserTokenValidity();

                if (!userTokenValidity.IsValid)
                {
                    return new ApiResponse<bool>()
                    {
                        Status = StatusCodes.Status403Forbidden,
                        Model = false
                    };
                }
                if (userTokenValidity.IsValid && userTokenValidity.ValidTo > DateTime.Now)
                {
                    var userId = GetUserId();
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                    return new ApiResponse<bool>()
                    {
                        Status = StatusCodes.Status200OK,
                        Model = true
                    };
                }
            }
            return new ApiResponse<bool>()
            {
                Status = StatusCodes.Status403Forbidden,
                Model = false
            };

            }
            catch
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status403Forbidden,
                    Model = false,
                };
            }
        }

        public string GetUserId()
        {
            var getAuthorizationToken = _httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues token);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var jwtUserToken = jsonToken as JwtSecurityToken;

            var tokenClaims = jwtUserToken.Claims.ToList();

            if (tokenClaims.Any(x => x.Type == "UserId"))
            {
                return tokenClaims.FirstOrDefault(x => x.Type == "UserId").Value.ToString();
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
