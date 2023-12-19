using AutoMapper;
using BASAccountManager.Controllers.Facebook.DTO;
using BASAccountManager.Controllers.Proxy.DTO;
using BASAccountManager.Controllers.SMS_Services.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager
{
    public class AutoMapperConf : Profile
    {
        public AutoMapperConf()
        {
            CreateMap<DBProxy, ProxyDTO>().ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.DBProxyGroup.Name));
            CreateMap<ProxyDTO, DBProxy>();
            CreateMap<DBFacebookAccount, FBAccountDTO>().ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.DBFBAccountGroup.Name));
            CreateMap<FBAccountDTO, DBFacebookAccount>();

            CreateMap<DBSMSActivation, SMSServiceDTO>().ReverseMap();
            CreateMap<DBTask, TaskDTO>().ReverseMap();
        }
    }
}