using AutoMapper;
using Freelance.Domain.Context;
using Freelance.Services.Models.Response;
using Freelance.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freelance.Services.Interfaces
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public NotificationService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<NotificationViewModel>>> GetByUser(string userProfileId)
        {
            if(string.IsNullOrEmpty(userProfileId))
            {
                return new ApiResponse<List<NotificationViewModel>> ()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ასეთი მომხმარებელი არ მოიძებნა"
                };
            }

            var notificationsOfUser = await _context.Notifications.Where(x => x.Status == Shared.Enumerations.EntityStatus.Active && x.UserId == userProfileId).ToListAsync();
            if (notificationsOfUser.Count() <= 0)
            {
                return new ApiResponse<List<NotificationViewModel>> ()
                {
                    Status = StatusCodes.Status200OK,
                    StatusMessage = "ნოტიფიკაციები არ მოიძებნა"
                };
            }

            var mapperResult = _mapper.Map<List<NotificationViewModel>>(notificationsOfUser);

            return new ApiResponse<List<NotificationViewModel>> ()
            {
                Status = StatusCodes.Status200OK,
                Model = mapperResult
            };
        }

        public async Task<ApiResponse<List<NotificationViewModel>>> GetForAdmin()
        {
            var notificationsOfUser = await _context.Notifications.Where(x => x.Status == Shared.Enumerations.EntityStatus.Active && x.UserType == Shared.Enumerations.UserType.Admin).ToListAsync();
            if (notificationsOfUser.Count() <= 0)
            {
                return new ApiResponse<List<NotificationViewModel>>()
                {
                    Status = StatusCodes.Status200OK,
                    StatusMessage = "ნოტიფიკაციები არ მოიძებნა"
                };
            }

            var mapperResult = _mapper.Map<List<NotificationViewModel>>(notificationsOfUser);

            return new ApiResponse<List<NotificationViewModel>>()
            {
                Status = StatusCodes.Status200OK,
                Model = mapperResult
            };
        }
    }
}
