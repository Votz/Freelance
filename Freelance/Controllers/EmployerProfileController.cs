using AutoMapper;
using Freelance.Api.Models.Request;
using Freelance.Services.Interfaces;
using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerProfileController : ControllerBase
    {
        private readonly IEmployerService _employerProfileService;
        private readonly IMapper _mapper;

        public EmployerProfileController(IEmployerService employerProfileService, IMapper mapper)
        {
            _employerProfileService = employerProfileService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<ApiResponse<PaginationResponseModel<EmployerProfileViewModel>>> GetAll([FromQuery] EmployerProfileFilterModel model)
        {
            var mappedResult = _mapper.Map<EmployerProfileModel>(model);
            return await _employerProfileService.GetAll(mappedResult);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse<int>> Create([FromBody] CreateEmployerProfileRequestModel model)
        {
            var mappedResult = _mapper.Map<EmployerProfileModel>(model);
            return await _employerProfileService.Create(mappedResult);
        }

        [HttpPost]
        public async Task<ApiResponse<int>> Update(CreateUserProfileRequestModel model)
        {
            var mappedResult = _mapper.Map<EmployerProfileModel>(model);
            return await _employerProfileService.Update(mappedResult);
        }

        [HttpGet]
        public async Task<ApiResponse<EmployerProfileViewModel>> Get(int id)
        {
            return await _employerProfileService.Get(id);
        }

        [HttpGet("[action]")]
        public async Task<ApiResponse> Delete(int id)
        {
            return await _employerProfileService.Delete(id);
        }
    }
}
