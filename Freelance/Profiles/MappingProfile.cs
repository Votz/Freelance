using AutoMapper;
using Freelance.Api.Models.Request;
using Freelance.Services.Models.Request;
using Freelance.Services.Models.Response;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Freelance.Api.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LoginRequest, LoginRequestModel>();
            CreateMap<LoginRequest, LoginRequestModel>().ReverseMap();

            CreateMap<CreateUserRequest, CreateuserModel>();
            CreateMap<CreateUserRequest, CreateuserModel>().ReverseMap();

            CreateMap<CreateRoleRequest, RoleModel>();
            CreateMap<CreateRoleRequest, RoleModel>().ReverseMap();

            CreateMap<AddUserInRoleRequest, AddUserInRoleModel>();
            CreateMap<AddUserInRoleRequest, AddUserInRoleModel>().ReverseMap();

            CreateMap<IdentityRole, RoleViewModel>();
            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();

            CreateMap<List<IdentityRole>, List<RoleViewModel>>();
            CreateMap<List<IdentityRole>, List<RoleViewModel>>().ReverseMap();

        }

    }
}
