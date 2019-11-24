using ApiAuth.API.Models;
using ApiAuth.API.ViewModels;
using AutoMapper;

namespace ApiAuth.API.Infrastructure.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, AppUserViewModel>();
        }
    }
}
