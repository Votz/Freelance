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
    public class UserCategoryService : IUserCategoryService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public UserCategoryService(ApplicationContext context,IMapper mapper,IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApiResponse<bool>> AddOrUpdate(UserCategoryModel model)
        {
            if(model == null)
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "მოთხოვნა არავალიდურია"
                };
            }

            if (model.CategoryIds.Count() <= 0 || string.IsNullOrEmpty(model.UserId))
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "მოთხოვნა არავალიდურია"
                };
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);

            if(user == null)
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "მომხმარებელი არ არსებობს"
                };
            }

            var categories = await _context.Categories.ToListAsync();

            if (!categories.Any(x => model.CategoryIds.Any(y => y == x.Id)))
            {
                return new ApiResponse<bool>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "კატეგორია არ მოიძებნა"
                };
            }

            var userCategories = await _context.UserCategories.Where(x => x.UserId == model.UserId).ToListAsync();

            if(userCategories.Count() > 0)
            {
                _context.RemoveRange(userCategories);
                await _context.SaveChangesAsync();
            }

            var newUserCategories = new List<UserCategory>();
            foreach (var userCategory in model.CategoryIds)
            {
                newUserCategories.Add(new UserCategory()
                {
                    UserId = model.UserId,
                    CategoryId = userCategory
                });
            }

            _context.UserCategories.AddRange(newUserCategories);

            await _context.SaveChangesAsync();


            return new ApiResponse<bool>()
            {
                Status = StatusCodes.Status200OK,
                Model = true
            };
        }

        public async Task<ApiResponse<List<UserCategoryViewModel>>> GetAll(UserCategoryFilterModel model)
        {
            var userCategories = await _context.UserCategories.Where(x => (string.IsNullOrEmpty(model.UserId) || x.UserId == model.UserId) &&
                                                                          (model.CategoryId.Count() <= 0 || model.CategoryId.Any(y => y == x.CategoryId))).ToListAsync();

            var viewModel = new List<UserCategoryViewModel>();

            if(userCategories.Count() <= 0)
            {
                return new ApiResponse<List<UserCategoryViewModel>>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ჩანაწერები არ მოიძებნა"
                };
            }
            var uniqueUserIds = userCategories.Select(x => x.UserId).Distinct().ToList();
            
            foreach(var userId in uniqueUserIds)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                viewModel.Add(new UserCategoryViewModel()
                {
                    Categories = userCategories.Where(x => x.UserId == userId).Select(y => new CategoryIdNameModel() {Id = y.CategoryId, Name = y.Category.Name }).ToList(),
                    Username = user.UserName,
                    UserId = userId
                });
            }

            return new ApiResponse<List<UserCategoryViewModel>>()
            {
                Status = StatusCodes.Status200OK,
                Model = viewModel
            };
        }

    }
}
