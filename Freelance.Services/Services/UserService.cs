using Freelance.Domain.Context;
using Freelance.Domain.Entities;
using Freelance.Services.Models.Request;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IPasswordValidatorService _validator;
        private readonly ApplicationContext _context;
        private readonly IAuthorizationService _authorizationService;


        public UserService(IAuthorizationService authorizationService, UserManager<User> userManager, IPasswordValidatorService validator, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
            _validator = validator;
            _authorizationService = authorizationService;
        }

        public async Task<ApiResponse<string>> Create(CreateuserModel model)
        {
            try
            {

                if (_context.Users.Any(x => x.Email == model.Email && x.UserName == model.UserName))
                {
                    return new ApiResponse<string>()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        StatusMessage = "მომხმარებელი მსგავსი იმეილით ან იუზერნეიმით უკვე არსებობს"
                    };
                }

                var passwordValidationErrors = _validator.Validate(model.Password);

                if (passwordValidationErrors.Count() <= 0)
                {
                    var user = new User()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Firstname = model.Firstname,
                        Lastname = model.Lastname
                    };

                    var createdResult = await _userManager.CreateAsync(user, model.ConfirmPassword);

                    if (createdResult.Succeeded)
                    {
                        if(model.UserType != 0)
                        {
                            var roleId = model.UserType == Shared.Enumerations.UserType.Employee ? _context.Roles.FirstOrDefault(x => x.Name == "employee").Id : _context.Roles.FirstOrDefault(x => x.Name == "employer").Id;
                            var addUserRole = await this.AddUserRole(new AddUserInRoleModel() { RoleId = roleId, UserId = user.Id });
                        }
                        return new ApiResponse<string>()
                        {
                            Status = StatusCodes.Status200OK,
                            Model = user.Id
                        };
                    }
                    return new ApiResponse<string>()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        StatusMessage = createdResult.Errors.FirstOrDefault().Description
                    };
                }
                else
                {
                    return new ApiResponse<string>()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        StatusMessage = "არავალიდური პაროლი"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    StatusMessage = "მომხმარებელი ვერ შეიქმნა,გთხოვთ სცადოთ მოგვიანებით"
                };
            }

        }

        public async Task<ApiResponse<bool>> AddUserRole(AddUserInRoleModel model)
        {
            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.RoleId))
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "არასწორი ინფორმაცია"
                };
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (user == null)
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "აღნიშნული მომხმარებელი არ მოიძებნა სისტემაში"
                };
            }


            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == model.RoleId);
            if (role == null)
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "აღნიშნული უფლება არ მოიძებნა სისტემაში"
                };
            }

            var assignResult = await _userManager.AddToRoleAsync(user, role.Name);

            if (assignResult.Succeeded)
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status200OK,
                    Model = true
                };
            }

            else
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Model = false,
                    Errors = new ApiError()
                    {
                        ErrorMessages = assignResult.Errors.Select(x => x.Description).ToList()
                    }
                };
            }
        }

    }
}
