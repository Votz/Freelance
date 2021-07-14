using AutoMapper;
using Freelance.Api.Models.Request;
using Freelance.Services.Interfaces;
using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IMapper _mapper;

        public UserProfileController(IUserProfileService userProfileService, IMapper mapper)
        {
            _userProfileService = userProfileService;
            _mapper = mapper;
        }


        [HttpGet("[action]")]
        public async Task<ApiResponse<PaginationResponseModel<UserProfileViewModel>>> GetAll([FromQuery] UserProfileFilterModel model)
        {
            var mappedResult = _mapper.Map<UserProfileModel>(model);
            return await _userProfileService.GetAll(mappedResult);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse<int>> Create([FromBody] CreateUserProfileRequestModel model)
        {
            var mappedResult = _mapper.Map<UserProfileModel>(model);
            return await _userProfileService.Create(mappedResult);
        }

        [HttpPost]
        public async Task<ApiResponse<int>> Update(CreateUserProfileRequestModel model)
        {
            var mappedResult = _mapper.Map<UserProfileModel>(model);
            return await _userProfileService.Update(mappedResult);
        }

        [HttpGet]
        public async Task<ApiResponse<UserProfileViewModel>> Get(int id)
        {
            return await _userProfileService.Get(id);
        }

        [HttpGet("[action]")]
        public async Task<ApiResponse> Delete(int id)
        {
            return await _userProfileService.Delete(id);
        }

    }
}
