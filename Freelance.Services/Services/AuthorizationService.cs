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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<AuthorizationService> _localizer;
        private readonly ILogger<IAuthorizationService> _logger;
        //private IOptions<Shared.Models.TokenOptions> _tokenOptionsConfiguration;

        public AuthorizationService(IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<IAuthorizationService> logger, ApplicationContext context, IStringLocalizer<AuthorizationService> localizer)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<ApiResponse<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await _context.Users.Include(x => x.UserProfile).Where(x => x.Email == model.Email).FirstOrDefaultAsync();

            if (user == null)
            {
                return new ApiResponse<LoginResponseModel>()
                {
                    Status = StatusCodes.Status404NotFound,
                    StatusMessage = "არასწორი ლოგინი ან პაროლი"
                };
            }

            var hashedPassword = _userManager.PasswordHasher.HashPassword(user, model.Password);

            if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, hashedPassword) != PasswordVerificationResult.Success)
            {

                user.AccessFailedCount++;

                await _context.SaveChangesAsync();
                return new ApiResponse<LoginResponseModel>()
                {
                    Status = StatusCodes.Status404NotFound,
                    StatusMessage = "არასწორი ლოგინი ან პაროლი"
                };
            }

            var claimedToken = await _userManager.AddClaimAsync(user, new Claim("ActivtationTokenExpiredAt", DateTime.Now.AddHours(1).ToString()));
            var token = await _userManager.GenerateUserTokenAsync(user, "FreelanceSite", "Authentication");
            if (string.IsNullOrEmpty(token))
            {
                return new ApiResponse<LoginResponseModel>()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    StatusMessage = "დაფიქსირდა შეცდომა მონაცემების დამუშავებისას"
                };
            }

            return new ApiResponse<LoginResponseModel>()
            {
                Status = StatusCodes.Status200OK,
                Model = new LoginResponseModel()
                {
                    Token = token,
                    TokenType = "Bearer",
                    ExpiryDate = DateTime.Now.AddMinutes(60),
                }
            };
        }

        public async Task<ApiResponse<LoginResponseModel>> LogInAsync(LoginRequestModel model)
        {
            var result = new ApiResponse<LoginResponseModel>();
            
            var user = _context.Users.Where(x => x.Email == model.Email).FirstOrDefault();

            if (user == null)
            {
                result.Status = StatusCodes.Status400BadRequest;
                result.Errors.ErrorMessages.Add("არასწორი ინფორმაცია");
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
                
            result.Model.Token = JwtHelper.Create(null, expires, userRoles, tokenOptions,claims);

            if (!string.IsNullOrEmpty(result.Model.Token))
            {
                _context.UserTokens.Add(new IdentityUserToken<string>()
                {
                    UserId = user.Id,
                    Name = user.UserName,
                    LoginProvider = "HireHaralo",
                    Value = result.Model.Token
                });
                await _context.SaveChangesAsync();
                return result;
            }
            result.Status = StatusCodes.Status400BadRequest;
            result.Errors.ErrorMessages.Add("არასწორი ინფორმაცია");
            return result;
        }

        public async Task<ApiResponse<bool>> Logout(string authorization)
        {
            //var userToken = GetUserToken();

            if (string.IsNullOrEmpty(authorization))
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status403Forbidden,
                    StatusMessage = "არაავტორიზირებული მოთხოვნა",
                    Model = false
                };
            }
            var tokenRecord = await _context.UserTokens.FirstOrDefaultAsync(x => x.Value == authorization);
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
            //var userToken = GetUserToken();
            //var claims = await _userManager.
            return null;
        }
        private string GetUserToken()
        {
            //var authorizationToken = HttpContext.GetTokenAsync("access_token");

            //if (string.IsNullOrEmpty(authorizationToken))
            //{
            //    return null;
            //}
            return null;
        }
    }
}
