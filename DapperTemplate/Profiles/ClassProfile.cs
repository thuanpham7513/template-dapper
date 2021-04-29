using AutoMapper;
using DapperTemplate.Models.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperTemplate.Profiles
{
    public class ClassProfile : Profile
    {
        public ClassProfile()
        {
            CreateMap<ClassRequestModel, Class>();
            CreateMap<Class, ClassResponseModel>();
        }
    }
}
