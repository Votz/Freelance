using AutoMapper;
using Freelance.Domain.Context;
using Freelance.Services.Interfaces;
using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public RoleService(RoleManager<IdentityRole> roleManager, ApplicationContext context,IMapper mapper)
        {
            _roleManager = roleManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<RoleViewModel>>> GetAll(RoleModel model)
        {
            var roleList = await _context.Roles.ToListAsync();

            var roleViewModelList = _mapper.Map<List<RoleViewModel>>(roleList);

            if(roleViewModelList.Count() <= 0)
            {
                return new ApiResponse<List<RoleViewModel>>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Model = null
                };
            }
            return new ApiResponse<List<RoleViewModel>>()
            {
                Status = StatusCodes.Status200OK,
                Model = roleViewModelList
            };
        }

        public async Task<ApiResponse<RoleViewModel>> Get(string id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (role == null)
            {
                return new ApiResponse<RoleViewModel>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Model = null
                };
            }

            return new ApiResponse<RoleViewModel>()
            {
                Status = StatusCodes.Status200OK,
                Model = _mapper.Map<RoleViewModel>(role)
            };
        }

        public async Task<ApiResponse<string>> Create(RoleModel model)
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

        public async Task<ApiResponse<string>> Update(RoleModel model)
        {
            if (_context.Roles.Any(x => x.Name == model.Name && x.Id != model.Id))
            {
                return new ApiResponse<string>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ასეთი უფლება უკვე არსებობს"
                };
            }
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (role == null)
            {
                return new ApiResponse<string>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "უფლება არ მოიძებნა"
                };
            }
            role.Name = model.Name;

            await _context.SaveChangesAsync();
            
            return new ApiResponse<string>()
            {
                Status = StatusCodes.Status200OK,
                Model = role.Id
            };
        }

        public async Task<ApiResponse> Delete(string id)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
            {
                return new ApiResponse<string>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "უფლება არ მოიძებნა"
                };
            }
            await _roleManager.DeleteAsync(role);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>()
            {
                Status = StatusCodes.Status200OK
            };
        }

    }
}
