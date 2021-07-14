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
    public class UserCategoryController : ControllerBase
    {
        private readonly IUserCategoryService _userCategory;
        private readonly IMapper _mapper;
        public UserCategoryController(IUserCategoryService userCategory, IMapper mapper)
        {
            _userCategory = userCategory;
            _mapper = mapper;

        }
        [HttpGet("[action]")]
        public async Task<ApiResponse<List<UserCategoryViewModel>>> GetAll([FromQuery] UserCategoryFilterRequestModel model)
        {
            var mappedResult = _mapper.Map<UserCategoryFilterModel>(model);
            return await _userCategory.GetAll(mappedResult);
        }

        [HttpPost("[action]")]
        public async Task<ApiResponse<bool>> AddOrUpdate([FromBody] UserCategoryFilterRequestModel model)
        {
            var mappedResult = _mapper.Map<UserCategoryModel>(model);
            return await _userCategory.AddOrUpdate(mappedResult);
        }

    }
}
