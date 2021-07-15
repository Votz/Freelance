using AutoMapper;
using Freelance.Domain.Context;
using Freelance.Domain.Entities;
using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Freelance.Shared.Enumerations;
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
    public class JobOfferService : IJobOfferService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public JobOfferService(ApplicationContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        public async Task<ApiResponse<PaginationResponseModel<JobOfferViewModel>>> GetAll(JobOfferModel model)
        {
            try
            {


                //var categoryId = model.Categories.Select(x => x.Id).ToList();
                var jobOfferList = await _context.JobOffers.Where(x => (string.IsNullOrEmpty(model.Name) || x.Name == model.Name) &&
                                                             (model.JobStatus == 0 || x.JobStatus == model.JobStatus)).ToListAsync();
                //&&
                //(model.Categories.Count == 0 || x.JobCategories.Any(y => categoryId.Contains(y.Category.Id)))).ToListAsync();

                if (jobOfferList.Count() <= 0)
                {
                    return new ApiResponse<PaginationResponseModel<JobOfferViewModel>>()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        StatusMessage = "ჩანაწერები არ მოიძებნა"
                    };
                }

                var jobOfferModelList = jobOfferList.Select(x => new JobOfferViewModel()
                {
                    Id = x.Id,
                    CreateDate = x.CreateDate,
                    JobStatus = x.JobStatus,
                    Description = x.Description,
                    Name = x.Name,
                }).ToList();
                var paginationViewModel = new PaginationResponseModel<JobOfferViewModel>(_context.JobOffers.Count(), jobOfferModelList);

                return new ApiResponse<PaginationResponseModel<JobOfferViewModel>>()
                {
                    Status = StatusCodes.Status200OK,
                    Model = paginationViewModel
                };
            }
            catch
            {
                return new ApiResponse<PaginationResponseModel<JobOfferViewModel>>()
                {
                    Status = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<ApiResponse<JobOfferViewModel>> Get(int id)
        {
            try
            {
                var jobOffer = await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == id);

                if (jobOffer == null)
                {
                    return new ApiResponse<JobOfferViewModel>()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Model = null
                    };
                }

                return new ApiResponse<JobOfferViewModel>()
                {
                    Status = StatusCodes.Status200OK,
                    Model = _mapper.Map<JobOfferViewModel>(jobOffer)
                };
            }
            catch
            {
                return new ApiResponse<JobOfferViewModel>()
                {
                    Status = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<ApiResponse<int>> Create(JobOfferModel model)
        {
            try
            {
                var jobOffers = await _context.JobOffers.ToListAsync();
                if (jobOffers.Any(x => x.Name == model.Name && x.EmployerId == model.EmployerId))
                {
                    return new ApiResponse<int>()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        StatusMessage = "ასეთი შეთავაზება მომხმარებელს უკვე გაკეთებული აქვს უკვე არსებობს"
                    };
                }

                var employer = _context.EmployerProfiles.FirstOrDefault(x => x.Id == model.EmployerId);
                if (employer == null)
                {
                    return new ApiResponse<int>()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        StatusMessage = "დმსაქმებლის პროფილი არ მოიძებნა"
                    };
                }

                var newJobOffer = _mapper.Map<JobOffer>(model);

                var authorizedUser = _authorizationService.GetUserId();
                newJobOffer.ModifierId = authorizedUser;

                await _context.JobOffers.AddAsync(newJobOffer);
                _context.SaveChanges();

                var newJobOfferCategories = model.Categories.Select(x => new JobCategory()
                {
                    CategoryId = x.Id,
                    JobOfferId = newJobOffer.Id,
                }).ToList();

                await _context.JobCategories.AddRangeAsync(newJobOfferCategories);
                _context.SaveChanges();

                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status200OK,
                    Model = newJobOffer.Id
                };
            }
            catch
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status500InternalServerError
                };
            }

        }

        public async Task<ApiResponse<int>> Update(JobOfferModel model)
        {
            var jobOffers = _context.JobOffers.ToList();
            var jobOffer = await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (jobOffers.Any(x => x.EmployerId == model.EmployerId && x.Name == model.Name && x.Id != model.Id))
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "მსგავსი სამსახურის შეთავაზება მომხმარებელს უკვე შექმნილი აქვს"
                };
            }

            if (jobOffer == null)
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "შეთავაზება არ მოიძებნა"
                };
            }

            _mapper.Map(model, jobOffer);

            var jobOfferCategory = await _context.JobCategories.Where(x => x.JobOfferId == jobOffer.Id).ToListAsync();
            _context.RemoveRange(jobOfferCategory);
            _context.SaveChanges();

            var newJobOfferCategories = model.Categories.Select(x => new JobCategory()
            {
                CategoryId = x.Id,
                JobOfferId = model.Id,
            }).ToList();

            await _context.JobCategories.AddRangeAsync(newJobOfferCategories);

            await _context.SaveChangesAsync();

            return new ApiResponse<int>()
            {
                Status = StatusCodes.Status200OK,
                Model = jobOffer.Id
            };
        }

        public async Task<ApiResponse> Delete(int id)
        {
            var jobOffer = await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == id);
            if (jobOffer == null)
            {
                return new ApiResponse()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "სამსახური არ მოიძებნა"
                };
            }

            jobOffer.Status = Shared.Enumerations.EntityStatus.Deleted;

            await _context.SaveChangesAsync();

            return new ApiResponse()
            {
                Status = StatusCodes.Status200OK
            };
        }

        public async Task<ApiResponse> ChangeJobStatus(ChangeJobOfferStatus model)
        {
            var jobOffer = await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == model.JobId);
            if (jobOffer == null)
            {
                return new ApiResponse()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ჩანაწერი არ მოიძებნა"
                };
            }
            if (model.Status == 0)
            {
                return new ApiResponse()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "სტატუსი არავალიდურია"
                };
            }
            jobOffer.JobStatus = model.Status;

            await _context.SaveChangesAsync();

            return new ApiResponse()
            {
                Status = StatusCodes.Status200OK
            };
        }

        //public async Task<ApiResponse<List<JobOfferViewModel>>> GetAll(JobOfferModel model)
        //{
        //    var bidList = await _context.JobOffer.Where(x => (string.IsNullOrEmpty(model.Name) || x.Name == model.Name) &&
        //                                                     (model.JobStatus == 0 || x.JobStatus == model.JobStatus) &&
        //                                                     (model.UserProfileId == 0 || x.UserProfileId == model.UserProfileId)).ToListAsync();

        //    if (bidList.Count() <= 0)
        //    {
        //        return new ApiResponse<List<BidViewModel>>()
        //        {
        //            Status = StatusCodes.Status400BadRequest,
        //            StatusMessage = "ჩანაწერები არ მოიძებნა"
        //        };
        //    }

        //    var bidViewModelList = _mapper.Map<List<BidViewModel>>(bidList);

        //    return new ApiResponse<List<BidViewModel>>()
        //    {
        //        Status = StatusCodes.Status200OK,
        //        Model = bidViewModelList
        //    };
        //}

        //public Task<ApiResponse<JobOfferViewModel>> Get(int id)
        //{
        //    throw new NotImplementedException();
        //}
        //public Task<ApiResponse<int>> Create(JobOfferModel model)
        //{
        //    throw new NotImplementedException();
        //}
        //public Task<ApiResponse<int>> Update(JobOfferModel model)
        //{
        //    throw new NotImplementedException();
        //}
        //public Task<ApiResponse> Delete(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
