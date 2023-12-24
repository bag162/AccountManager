using AutoMapper;
using BASAccountManager.Controllers.BASTask.DTO;
using BASAccountManager.Controllers.Email.DTO;
using BASAccountManager.Controllers.Instagram.DTO;
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

            CreateMap<DBInstagramAccount, InstAccountDTO>().ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.DBInstAccountGroup.Name));
            CreateMap<InstAccountDTO, DBInstagramAccount>();

            CreateMap<DBWorkerTask, WorkerTaskDTO>().ForMember(dest => dest.InstAccountLogin, opt => opt.MapFrom(src => src.InstAccount.Login));
            CreateMap<WorkerTaskDTO, DBWorkerTask>();

            CreateMap<DBEmail, EmailDTO>().ReverseMap();
            CreateMap<DBSMSActivation, SMSServiceDTO>().ReverseMap();
            CreateMap<DBTask, TaskDTO>().ReverseMap();
            CreateMap<DBInstagramAccount, EndRegistrationTaskDTO>().ReverseMap();

            CreateMap<DBWorkerTask, GetRegistrationTaskEmailServiceDTO>().ForMember(dest => dest.UsefulData, opt => opt.Ignore());
            CreateMap<DBWorkerTask, GetRegistrationTaskSMSServiceDTO>().ForMember(dest => dest.UsefulData, opt => opt.Ignore());
        }
    }
}