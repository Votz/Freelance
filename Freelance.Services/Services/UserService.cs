using Freelance.Domain.Context;
using Freelance.Domain.Entities;
using Freelance.Services.Models.Request;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;

        public UserService(UserManager<User> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<ApiResponse<string>> Create(CreateuserModel model)
        {
            if (_context.Users.Any(x => x.Email == model.Email && x.UserName == model.UserName))
            {
                return new ApiResponse<string>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "მომხმარებელი მსგავსი იმეილით ან იუზერნეიმით უკვე არსებობს"
                };
            }

            var passwordValidator = new PasswordValidator<User>();
            var result = await passwordValidator.ValidateAsync(_userManager, null, model.Password);

            if (result.Succeeded)
            {
                var user = new User()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                var createdResult = await _userManager.CreateAsync(user,model.ConfirmPassword);

                if (createdResult.Succeeded)
                {
                    return new ApiResponse<string>()
                    {
                        Status = StatusCodes.Status200OK,
                        Model = user.Id
                    };
                }
                return new ApiResponse<string>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "უფლება ვერ შეიქმნა"
                };
            }
            else
            {
                return new ApiResponse<string>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Model = "არავალიდური პაროლი"
                };
            }

        }
    }
}
