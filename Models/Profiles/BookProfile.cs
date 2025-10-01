using AutoMapper;
using Library_Management_Sys.Models;
using Library_Management_Sys.Models.DTOs;

namespace Library_Management_Sys.Models.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
        }
    }
}