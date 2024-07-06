using AutoMapper;
using HrSystem.DAL.Entities;
using HrSystem.PL.Models;

namespace HRSystem.PL.Mapper
{
    public class GeneralSettingProfile:Profile
    {
        public GeneralSettingProfile()
        {
            CreateMap < GeneralSetting,GeneralSettingViewModel>().ReverseMap();
        }
    }
}
