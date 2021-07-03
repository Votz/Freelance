using AutoMapper;
using Freelance.Domain.Context;
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
    public class JobOfferService : IJobOfferService
    {
        private readonly IJobOfferService _jobOfferService;
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public JobOfferService(IJobOfferService jobOfferService, ApplicationContext context, IMapper mapper)
        {
            _bidService = bidService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<JobOfferViewModel>>> GetAll(JobOfferModel model)
        {
            var categoryId = model.Categories.Select(x => x.Id).ToList();
            var jobOfferList = await _context.JobOffers.Where(x => (string.IsNullOrEmpty(model.Name) || x.Name == model.Name) &&
                                                         (model.JobStatus == 0 || x.JobStatus == model.JobStatus) &&
                                                         (model.Categories.Count == 0 || x.JobCategories.Any(y => categoryId.Contains(y.Category.Id)))).ToListAsync();

            if (jobOfferList.Count() <= 0)
            {
                return new ApiResponse<List<JobOfferViewModel>>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ჩანაწერები არ მოიძებნა"
                };
            }

            var bidViewModelList = _mapper.Map<List<JobOfferViewModel>>(jobOfferList);

            return new ApiResponse<List<JobOfferViewModel>>()
            {
                Status = StatusCodes.Status200OK,
                Model = bidViewModelList
            };
        }

        public async Task<ApiResponse<JobOfferViewModel>> Get(int id)
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

        public async Task<ApiResponse<int>> Create(JobOfferModel model)
        {
            var jobOffers = await _context.JobOffers.ToListAsync();
            if (jobOffers.Any(x => x.Name == model.Name && x. == model.JobId))
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ასეთი შეთავაზება მომხმარებელს უკვე გაკეთებული აქვს უკვე არსებობს"
                };
            }
            var newBid = _mapper.Map<Bid>(model);

            _context.Bids.Add(newBid);
            await _context.SaveChangesAsync();

            return new ApiResponse<int>()
            {
                Status = StatusCodes.Status200OK,
                Model = newBid.Id
            };
        }

        public async Task<ApiResponse<int>> Update(JobOfferModel model)
        {

            var bids = await _context.Bids.ToListAsync();

            if (bids.Any(x => x.UserProfileId == model.UserProfileId && x.JobId == model.JobId && x.Id != model.Id))
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ასეთი შეთავაზება მომხმარებელს უკვე გაკეთებული აქვს"
                };
            }

            var bid = await _context.Bids.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (bid == null)
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "შეთავაზება არ მოიძებნა"
                };
            }

            _mapper.Map(model, bid);

            await _context.SaveChangesAsync();

            return new ApiResponse<int>()
            {
                Status = StatusCodes.Status200OK,
                Model = bid.Id
            };
        }

        public async Task<ApiResponse> Delete(int id)
        {
            var bid = await _context.Bids.FirstOrDefaultAsync(x => x.Id == id);
            if (bid == null)
            {
                return new ApiResponse()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "შეთავაზება არ მოიძებნა"
                };
            }
            bid.Status = Shared.Enumerations.EntityStatus.Deleted;

            await _context.SaveChangesAsync();

            return new ApiResponse()
            {
                Status = StatusCodes.Status200OK
            };
        }





        public async Task<ApiResponse<List<JobOfferViewModel>>> GetAll(JobOfferModel model)
        {
            var bidList = await _context.JobOffer.Where(x => (string.IsNullOrEmpty(model.Name) || x.Name == model.Name) &&
                                                             (model.JobStatus == 0 || x.JobStatus == model.JobStatus) &&
                                                             (model.UserProfileId == 0 || x.UserProfileId == model.UserProfileId)).ToListAsync();

            if (bidList.Count() <= 0)
            {
                return new ApiResponse<List<BidViewModel>>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ჩანაწერები არ მოიძებნა"
                };
            }

            var bidViewModelList = _mapper.Map<List<BidViewModel>>(bidList);

            return new ApiResponse<List<BidViewModel>>()
            {
                Status = StatusCodes.Status200OK,
                Model = bidViewModelList
            };
        }

        public Task<ApiResponse<JobOfferViewModel>> Get(int id)
        {
            throw new NotImplementedException();
        }
        public Task<ApiResponse<int>> Create(JobOfferModel model)
        {
            throw new NotImplementedException();
        }
        public Task<ApiResponse<int>> Update(JobOfferModel model)
        {
            throw new NotImplementedException();
        }
        public Task<ApiResponse> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
