using Freelance.Domain.Context;
using Freelance.Domain.Entities;
using Freelance.Services.Interfaces;
using Freelance.Services.Models.Request;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;
        private readonly IPasswordValidatorService _validator;
        private readonly IAuthorizationService _authorization;
        
        public PasswordResetService(IAuthorizationService authorization, IPasswordValidatorService validator, UserManager<User> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
            _validator = validator;
            _authorization = authorization;
        }
        public async Task<ApiResponse<bool>> ResetPassword(ResetPasswordModel model)
        {
            var authorization = _authorization.GetUserToken();
            if (string.IsNullOrEmpty(authorization))
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status403Forbidden,
                    StatusMessage = StatusCodes.Status403Forbidden.ToString()
                };
            }
            var userId = _authorization.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status403Forbidden,
                    StatusMessage = StatusCodes.Status403Forbidden.ToString()
                };
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            var passwordValidator = _validator.Validate(model.ConfirmNewPassword);

            if (passwordValidator.Count() <= 0)
            {
                var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordResult = await _userManager.ResetPasswordAsync(user, resetPasswordToken, model.ConfirmNewPassword);
                if (resetPasswordResult.Succeeded)
                {
                    return new ApiResponse<bool>()
                    {
                        Status = StatusCodes.Status200OK,
                        Model = true,
                    };
                }
                else
                {
                    return new ApiResponse<bool>()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Model = false,
                        StatusMessage = "პაროლის შეცვლა წარუმატებლად დასრულდა,გთხოვთ სცადოთ ხელახლა"

                    };
                }
            }
            else
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Model = false,
                    StatusMessage = "არავალიდური პაროლი",
                    Errors = new ApiError()
                    {
                        ErrorMessages = passwordValidator
                    }
                };
            }
        }
    }
}
