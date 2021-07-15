using AutoMapper;
using Freelance.Domain.Context;
using Freelance.Domain.Entities;
using Freelance.Services.Interfaces;
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

namespace Freelance.Services.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public EmployerService(ApplicationContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        public async Task<ApiResponse<PaginationResponseModel<EmployerProfileViewModel>>> GetAll(EmployerProfileModel model)
        {
            try
            {


                var employerProfileList = await _context.EmployerProfiles.Where(x => (string.IsNullOrEmpty(model.UserId) || x.UserId == model.UserId) &&
                                                             (model.Rating == 0 || x.Rating == model.Rating) &&
                                                             (string.IsNullOrEmpty(model.Name) || x.Name.Contains(model.Name))).ToListAsync();

                if (employerProfileList.Count() <= 0)
                {
                    return new ApiResponse<PaginationResponseModel<EmployerProfileViewModel>>()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        StatusMessage = "ჩანაწერები არ მოიძებნა"
                    };
                }

                var employerProfileModelList = _mapper.Map<List<EmployerProfileViewModel>>(employerProfileList);
                var paginationViewModel = new PaginationResponseModel<EmployerProfileViewModel>(_context.EmployerProfiles.Count(), employerProfileModelList);

                return new ApiResponse<PaginationResponseModel<EmployerProfileViewModel>>()
                {
                    Status = StatusCodes.Status200OK,
                    Model = paginationViewModel
                };
            }
            catch
            {
                return new ApiResponse<PaginationResponseModel<EmployerProfileViewModel>>()
                {
                    Status = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<ApiResponse<EmployerProfileViewModel>> Get(int id)
        {
            try
            {

                var userId = _authorizationService.GetUserId();

                var employerProfile = id <= 0 ? await _context.EmployerProfiles.FirstOrDefaultAsync(x => x.UserId == userId) : await _context.EmployerProfiles.FirstOrDefaultAsync(x => x.Id == id);

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == employerProfile.UserId);

                if (employerProfile == null)
                {
                    return new ApiResponse<EmployerProfileViewModel>()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Model = null
                    };
                }

                return new ApiResponse<EmployerProfileViewModel>()
                {
                    Status = StatusCodes.Status200OK,
                    Model = new EmployerProfileViewModel()
                    {
                        Id = employerProfile.Id,
                        Name = employerProfile.Name,
                        Rating = employerProfile.Rating,
                        Address = employerProfile.Address,
                        Description = employerProfile.Description,
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email
                    }
                };
            }
            catch
            {
                return new ApiResponse<EmployerProfileViewModel>()
                {
                    Status = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<ApiResponse<int>> Create(EmployerProfileModel model)
        {
            var employerProfiles = await _context.EmployerProfiles.ToListAsync();
            if (employerProfiles.Any(x => x.UserId == model.UserId))
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "მომხმარებელს უკვე დამატებული აქვს პროფილის ინფორმაცია"
                };
            }
            //var newEmployerProfile = _mapper.Map<EmployerProfile>(model);
            //newEmployerProfile.ModifierId = _authorizationService.GetUserId();
            var newEmployerProfile = new EmployerProfile()
            {
                Name = model.Name,
                Rating = model.Rating,
                Description = model.Description,
                Address = model.Address,
                UserId = model.UserId,
            };

            _context.EmployerProfiles.Add(newEmployerProfile);
            await _context.SaveChangesAsync();

            return new ApiResponse<int>()
            {
                Status = StatusCodes.Status200OK,
                Model = newEmployerProfile.Id
            };
        }

        public async Task<ApiResponse<int>> Update(EmployerProfileModel model)
        {
            var employerProfile = await _context.EmployerProfiles.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (employerProfile == null)
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "მომხმარებლის პროფილი არ მოიძებნა"
                };
            }

            _mapper.Map(model, employerProfile);
            employerProfile.ModifierId = _authorizationService.GetUserId();
            await _context.SaveChangesAsync();

            return new ApiResponse<int>()
            {
                Status = StatusCodes.Status200OK,
                Model = employerProfile.Id
            };
        }

        public async Task<ApiResponse> Delete(int id)
        {
            var employerProfile = await _context.EmployerProfiles.FirstOrDefaultAsync(x => x.Id == id);
            if (employerProfile == null)
            {
                return new ApiResponse()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "შეთავაზება არ მოიძებნა"
                };
            }
            employerProfile.Status = Shared.Enumerations.EntityStatus.Deleted;

            await _context.SaveChangesAsync();

            return new ApiResponse()
            {
                Status = StatusCodes.Status200OK
            };
        }
    }
}
