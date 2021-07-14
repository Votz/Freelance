using AutoMapper;
using Freelance.Domain.Context;
using Freelance.Domain.Entities;
using Freelance.Services.Models.Request;
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
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public UserProfileService(ApplicationContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        public async Task<ApiResponse<PaginationResponseModel<UserProfileViewModel>>> GetAll(UserProfileModel model)
        {
            var userProfileList = await _context.UserProfiles.Where(x => (string.IsNullOrEmpty(model.UserId) || x.UserId == model.UserId) &&
                                                         (model.Rating == 0 || x.Rating == model.Rating) &&
                                                         (model.HourRate == 0 || x.HourRate == model.HourRate)).ToListAsync();

            if (userProfileList.Count() <= 0)
            {
                return new ApiResponse<PaginationResponseModel<UserProfileViewModel>>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ჩანაწერები არ მოიძებნა"
                };
            }

            var bidViewModelList = _mapper.Map<List<UserProfileViewModel>>(userProfileList);
            var paginationViewModel = new PaginationResponseModel<UserProfileViewModel>(_context.JobOffers.Count(), bidViewModelList);

            return new ApiResponse<PaginationResponseModel<UserProfileViewModel>>()
            {
                Status = StatusCodes.Status200OK,
                Model = paginationViewModel
            };
        }

        public async Task<ApiResponse<UserProfileViewModel>> Get(int id)
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == id);

            if (userProfile == null)
            {
                return new ApiResponse<UserProfileViewModel>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Model = null
                };
            }

            return new ApiResponse<UserProfileViewModel>()
            {
                Status = StatusCodes.Status200OK,
                Model = _mapper.Map<UserProfileViewModel>(userProfile)
            };
        }

        public async Task<ApiResponse<int>> Create(UserProfileModel model)
        {
            var userProfiles = await _context.UserProfiles.ToListAsync();
            if (userProfiles.Any(x => x.UserId == model.UserId))
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ასეთი შეთავაზება მომხმარებელს უკვე გაკეთებული აქვს ამ სამსახურზე"
                };
            }
            var newUserProfile = _mapper.Map<UserProfile>(model);
            newUserProfile.ModifierId = _authorizationService.GetUserId();
            _context.UserProfiles.Add(newUserProfile);
            await _context.SaveChangesAsync();

            return new ApiResponse<int>()
            {
                Status = StatusCodes.Status200OK,
                Model = newUserProfile.Id
            };
        }

        public async Task<ApiResponse<int>> Update(UserProfileModel model)
        {

            //var userProfiles = await _context.UserProfiles.ToListAsync();

            //if (bids.Any(x => x.UserId == model.UserId && x.Id != model.Id))
            //{
            //    return new ApiResponse<int>()
            //    {
            //        Status = StatusCodes.Status400BadRequest,
            //        StatusMessage = "მომხმარებლის პროფილი უკვე მომხმარებელს უკვე გაკეთებული აქვს"
            //    };
            //}

            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (userProfile == null)
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "მომხმარებლის პროფილი არ მოიძებნა"
                };
            }

            _mapper.Map(model, userProfile);
            userProfile.ModifierId = _authorizationService.GetUserId();
            await _context.SaveChangesAsync();

            return new ApiResponse<int>()
            {
                Status = StatusCodes.Status200OK,
                Model = userProfile.Id
            };
        }

        public async Task<ApiResponse> Delete(int id)
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.Id == id);
            if (userProfile == null)
            {
                return new ApiResponse()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "შეთავაზება არ მოიძებნა"
                };
            }
            userProfile.Status = Shared.Enumerations.EntityStatus.Deleted;

            await _context.SaveChangesAsync();

            return new ApiResponse()
            {
                Status = StatusCodes.Status200OK
            };
        }
    }
}
