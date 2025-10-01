using AutoMapper;
using Library_Management_Sys.Models;
using Library_Management_Sys.Models.DTOs;

namespace Library_Management_Sys.Models.Profiles
{
    public class BorrowTransactionProfile : Profile
    {
        public BorrowTransactionProfile()
        {
            CreateMap<BorrowTransaction, BorrowTransactionDTO>().ReverseMap();
        }
    }
}