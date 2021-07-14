using AutoMapper;
using Freelance.Services.Interfaces;
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
            var mappedResult = _mapper.Map<BidModel>(model);
            return await _bidService.GetAll(mappedResult);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse<int>> Create([FromBody] CreateBidRequest model)
        {
            var mappedResult = _mapper.Map<BidModel>(model);
            return await _bidService.Create(mappedResult);
        }

        [HttpPost]
        public async Task<ApiResponse<int>> Update(BidFilterModel model)
        {
            var mappedResult = _mapper.Map<BidModel>(model);
            return await _bidService.Update(mappedResult);
        }

        [HttpGet]
        public async Task<ApiResponse<BidViewModel>> Get(int id)
        {
            return await _bidService.Get(id);
        }

        [HttpGet("[action]")]
        public async Task<ApiResponse> Delete(int id)
        {
            return await _bidService.Delete(id);
        }

    }
}
