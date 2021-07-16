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
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;


        public CategoryService(ApplicationContext context, IMapper mapper, IAuthorizationService authorizationService)
        {
            _mapper = mapper;
            _context = context;
            _authorizationService = authorizationService;
        }

        public async Task<ApiResponse<List<CategoryViewModel>>> GetAll()
        {
            var categoryList = await _context.Categories.ToListAsync();

            //= _mapper.Map<List<CategoryViewModel>>(categoryList);
            var categoryViewModelList = categoryList.Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Status = x.Status,
            }).ToList();


            if (categoryViewModelList.Count() <= 0)
            {
                return new ApiResponse<List<CategoryViewModel>>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Model = null
                };
            }

            return new ApiResponse<List<CategoryViewModel>>()
            {
                Status = StatusCodes.Status200OK,
                Model = categoryViewModelList
            };
        }

        public async Task<ApiResponse<CategoryViewModel>> Get(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return new ApiResponse<CategoryViewModel>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Model = null
                };
            }

            return new ApiResponse<CategoryViewModel>()
            {
                Status = StatusCodes.Status200OK,
                Model = new CategoryViewModel() {Id = category.Id, Name = category.Name, Status = category.Status }
            };
        }

        public async Task<ApiResponse<int>> Create(CategoryModel model)
        {
            try
            {
                if (_context.Categories.Any(x => x.Name == model.Name))
                {
                    return new ApiResponse<int>()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        StatusMessage = "ასეთი კატეგორია უკვე არსებობს"
                    };
                }

                var newCategory = new Category()
                {
                    Name = model.Name
                };
                await _context.Categories.AddAsync(newCategory);
                await _context.SaveChangesAsync();

                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status200OK,
                    Model = newCategory.Id
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

        public async Task<ApiResponse> Delete(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return new ApiResponse()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "კატეგორია არ მოიძებნა"
                };
            }
            category.Status = EntityStatus.Deleted;
            await _context.SaveChangesAsync();

            return new ApiResponse()
            {
                Status = StatusCodes.Status200OK
            };
        }

        public async Task<ApiResponse<int>> Update(CategoryModel model)
        {
            if (_context.Categories.Any(x => x.Name == model.Name && x.Id != model.Id))
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status400BadRequest,
                    StatusMessage = "ასეთი კატეგორია უკვე არსებობს"
                };
            }
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (category == null)
            {
                return new ApiResponse<int>()
                {
                    Status = StatusCodes.Status404NotFound,
                    StatusMessage = "კატეგორია არ მოიძებნა"
                };
            }
            category.Name = model.Name;
            category.Status = model.Status;
            category.ModifierId = _authorizationService.GetUserId();
            await _context.SaveChangesAsync();

            return new ApiResponse<int>()
            {
                Status = StatusCodes.Status200OK,
                Model = category.Id
            };
        }
    }
}
