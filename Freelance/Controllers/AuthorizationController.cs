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
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        public AuthorizationController(IAuthorizationService authorizationService, IMapper mapper)
        {
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ApiResponse<LoginResponseModel>> Login([FromBody]LoginRequest model)
        {
            var mappedResult = _mapper.Map<LoginRequestModel>(model);
            return await _authorizationService.Login(mappedResult);
        }
    }
}
