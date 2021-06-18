using AutoMapper;
using Freelance.Api.Models.Request;
using Freelance.Services.Models.Request;
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

            CreateMap<CreateRoleRequest, CreateRoleModel>();
            CreateMap<CreateRoleRequest, CreateRoleModel>().ReverseMap();
        }
        
    }
}
