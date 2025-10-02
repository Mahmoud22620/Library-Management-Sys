using AutoMapper;
using Library_Management_Sys.Models;
using Library_Management_Sys.Models.DTOs;

namespace Library_Management_Sys.Models.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => string.Join(", ", src.Authors.Select(a => a.Author.Name))))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher.Name));
            CreateMap<Book, BookUpdateDTO>().ReverseMap();
        }
    }
}