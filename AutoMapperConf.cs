using AutoMapper;
using BASAccountManager.Controllers.Facebook.DTO;
using BASAccountManager.Controllers.Proxy.DTO;
using BASAccountManager.Controllers.SMS_Services.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager
{
    public class AutoMapperConf : Profile
    {
        public AutoMapperConf()
        {
            CreateMap<DBProxy, ProxyDTO>().ReverseMap();
            CreateMap<DBFacebookAccount, FBAccountDTO>().ReverseMap();
            CreateMap<DBSMSActivation, SMSServiceDTO>().ReverseMap();
        }
    }
}