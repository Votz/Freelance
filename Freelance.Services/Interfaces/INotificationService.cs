using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ApiResponse<List<NotificationViewModel>>> GetByUser(string userProfileId);
        Task<ApiResponse<List<NotificationViewModel>>> GetForAdmin();
    }
}
