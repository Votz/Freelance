using Freelance.Api.Filters;
using Freelance.Services.Interfaces;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Freelance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("[action]")]
        public async Task<ApiResponse<List<NotificationViewModel>>> GetForUser( string userProfileId)
        {
            return await _notificationService.GetByUser(userProfileId);
        }

        [HttpGet("[action]")]
        [AuthorizeApi(Roles = "Admin")]
        public async Task<ApiResponse<List<NotificationViewModel>>> GetForAdmin()
        {
            return await _notificationService.GetForAdmin();
        }
    }
}
