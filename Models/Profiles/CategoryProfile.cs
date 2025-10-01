using AutoMapper;
using Library_Management_Sys.Models.DTOs;

namespace Library_Management_Sys.Models.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }

}
