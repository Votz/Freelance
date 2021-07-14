using AutoMapper;
using Freelance.Api.Models.Request;
using Freelance.Services.Interfaces;
using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Freelance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<ApiResponse<PaginationResponseModel<RoleViewModel>>> GetAll([FromQuery] RoleFilterModel model)
        {
            var mappedResult = _mapper.Map<RoleModel>(model);
            return await _roleService.GetAll(mappedResult);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse<string>> Create([FromBody] CreateRoleRequest model)
        {
            var mappedResult = _mapper.Map<RoleModel>(model);
            return await _roleService.Create(mappedResult);
        }

        [HttpPost]  
        public async Task<ApiResponse<string>> Update(RoleFilterModel model)
        {
            var mappedResult = _mapper.Map<RoleModel>(model);
            return await _roleService.Update(mappedResult);
        }

        [HttpGet]
        public async Task<ApiResponse<RoleViewModel>> Get(string id)
        {
            return await _roleService.Get(id);
        }

        [HttpGet("[action]")]
        public async Task<ApiResponse> Delete(string id)
        {
            return await _roleService.Delete(id);
        }

    }
}
