using AutoMapper;
using Library_Management_Sys.Models.DTOs;

namespace Library_Management_Sys.Models.Profiles
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap<UserActivityLog, ActivityLogDTO>().ReverseMap();

        }
    }
}
