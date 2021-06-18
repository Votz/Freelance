using Freelance.Domain.Context;
using Freelance.Domain.Entities;
using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationContext _context;
        private readonly IStringLocalizer<AuthorizationService> _localizer;

        public AuthorizationService(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext context, IStringLocalizer<AuthorizationService> localizer)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
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

        public async Task<ApiResponse<bool>> Logout()
        {
            var userToken = GetUserToken();

            if(userToken == null)
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status403Forbidden,
                    StatusMessage = "არაავტორიზირებული მოთხოვნა",
                    Model = false
                };
            }
            await _signInManager.SignOutAsync();

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
            var authorizationToken = System.Net.HttpRequestHeader.Authorization.ToString();

            if (string.IsNullOrEmpty(authorizationToken))
            {
                return null;
            }
            return authorizationToken;
        }
    }
}
