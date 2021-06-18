using Freelance.Domain.Context;
using Freelance.Services.Interfaces;
using Freelance.Services.Models.Request;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;

        public RoleService(RoleManager<IdentityRole> roleManager, ApplicationContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<ApiResponse<string>> Create(CreateRoleModel model)
        {
            if(_context.Roles.Any(x => x.Name == model.Name))
            {
                return new ApiResponse<string>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ასეთი უფლება უკვე არსებობს"
                };
            }
            var newRole = new IdentityRole()
            {
                Name = model.Name
            };
            var createdResult = await _roleManager.CreateAsync(newRole);

            if (createdResult.Succeeded)
            {
                return new ApiResponse<string>()
                {
                    Status = StatusCodes.Status200OK,
                    Model = newRole.Id
                };
            }
            return new ApiResponse<string>()
            {
                Status = StatusCodes.Status400BadRequest,
                StatusMessage = "უფლება ვერ შეიქმნა"
            };
        }
    }
}
