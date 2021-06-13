using Freelance.Domain.Context;
using Freelance.Domain.Entities;
using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Freelance.Services.Interfaces
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;
        private readonly IStringLocalizer<AuthorizationService> _localizer;

        public AuthorizationService(UserManager<User> userManager, ApplicationContext context, IStringLocalizer<AuthorizationService> localizer)
        {
            _userManager = userManager;
            _context = context;
        }

        public ApiResponse<LoginResponseModel> Login(LoginRequestModel model)
        {
            var user = _context.Users.Where(x => x.Email == model.Email).FirstOrDefault();
            if(user == null)
            {
                return new ApiResponse<LoginResponseModel>()
                {
                    Status = StatusCodes.Status404NotFound,
                    StatusMessage = _localizer["Hello"]
                };
            }
            return null;
        }
    }
}
