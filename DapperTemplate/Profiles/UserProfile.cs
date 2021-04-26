using AutoMapper;
using DapperTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTemplate.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponseModel>();

            CreateMap<UserRequestModel, User>();
        }
    }
}
