using AutoMapper;
using Freelance.Api.Models.Request;
using Freelance.Services.Interfaces;
using Freelance.Services.Models.Request;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Freelance.Api.Filters;

namespace Freelance.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
         
        //[AuthorizeApi(Roles = "Employer")]
        [HttpPost]
        public async Task<ApiResponse<string>> Create([FromBody] CreateUserRequest model)
        {
            var mappedResult = _mapper.Map<CreateuserModel>(model);
            return await _userService.Create(mappedResult);
        }

        [HttpPost("UserRole")]
        public async Task<ApiResponse<bool>> UserRole([FromBody] AddUserInRoleRequest model)
        {
            var mappedResult = _mapper.Map<AddUserInRoleModel>(model);
            return await _userService.AddUserRole(mappedResult);
        }
    }
}
