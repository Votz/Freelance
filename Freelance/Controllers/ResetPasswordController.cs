using AutoMapper;
using Freelance.Api.Models.Request;
using Freelance.Services.Interfaces;
using Freelance.Services.Models.Request;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Freelance.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        private readonly IPasswordResetService _passwordResetService;
        private readonly IMapper _mapper;
        public ResetPasswordController(IPasswordResetService passwordResetService, IMapper mapper)
        {
            _passwordResetService = passwordResetService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ApiResponse<bool>> ResetPassword([FromBody] ResetPasswordRequestModel model)
        {
            var mappedResult = _mapper.Map<ResetPasswordModel>(model);
            return await _passwordResetService.ResetPassword(mappedResult);
        }
    }
}
