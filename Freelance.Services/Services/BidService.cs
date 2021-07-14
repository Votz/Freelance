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
    public class BidService : IBidService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public BidService(ApplicationContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            _context = context;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        public async Task<ApiResponse<PaginationResponseModel<BidViewModel>>> GetAll(BidModel model)
        {
            var bidList = await _context.Bids.Where(x => (model.JobId == 0 || x.JobId == model.JobId) &&
                                                         (model.Rate == 0 || x.Rate == model.Rate) &&
                                                         (model.UserProfileId == 0 || x.UserProfileId == model.UserProfileId)).ToListAsync();

            if(bidList.Count() <= 0)
            {
                return new ApiResponse<PaginationResponseModel<BidViewModel>>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ჩანაწერები არ მოიძებნა"
                };
            }

            var bidViewModelList = _mapper.Map<List<BidViewModel>>(bidList);
            var paginationViewModel = new PaginationResponseModel<BidViewModel>(_context.JobOffers.Count(), bidViewModelList);

            return new ApiResponse<PaginationResponseModel<BidViewModel>>()
            {
                Status = StatusCodes.Status200OK,
                Model = paginationViewModel
            };
        }

        public async Task<ApiResponse<BidViewModel>> Get(int id)
        {
            var bid = await _context.Bids.FirstOrDefaultAsync(x => x.Id == id);

            if (bid == null)
            {
                return new ApiResponse<BidViewModel>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Model = null
                };
            }

            return new ApiResponse<BidViewModel>()
            {
                Status = StatusCodes.Status200OK,
                Model = _mapper.Map<BidViewModel>(bid)
            };
        }

        public async Task<ApiResponse<int>> Create(BidModel model)
        {
            var bids = await _context.Bids.ToListAsync();
            if (bids.Any(x => x.UserProfileId == model.UserProfileId && x.JobId == model.JobId))
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ასეთი შეთავაზება მომხმარებელს უკვე გაკეთებული აქვს ამ სამსახურზე"
                };
            }
            var newBid = _mapper.Map<Bid>(model);
            newBid.ModifierId = _authorizationService.GetUserId();
            _context.Bids.Add(newBid);
            await _context.SaveChangesAsync();

            return new ApiResponse<int>()
            {
                Status = StatusCodes.Status200OK,
                Model = newBid.Id
            };
        }

        public async Task<ApiResponse<int>> Update(BidModel model)
        {

            var bids = await _context.Bids.ToListAsync();

            if(bids.Any(x => x.UserProfileId == model.UserProfileId && x.JobId == model.JobId && x.Id != model.Id))
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
            bid.ModifierId = _authorizationService.GetUserId();
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

    }
}
