using AutoMapper;
using BASAccountManager.Controllers.BASTask.DTO;
using BASAccountManager.Controllers.Email.DTO;
using BASAccountManager.Controllers.Instagram.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.Controllers.Post.Group.DTO;
using BASAccountManager.Controllers.Post.Like.DTO;
using BASAccountManager.Controllers.Post.Post.DTO;
using BASAccountManager.Controllers.Proxy.DTO;
using BASAccountManager.Controllers.SMS_Services.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.Post;

namespace BASAccountManager
{
    public class AutoMapperConf : Profile
    {
        public AutoMapperConf()
        {
            // Default controllers Mapping
            CreateMap<DBProxy, ProxyDTO>().ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.ProxyGroup.Name));
            CreateMap<ProxyDTO, DBProxy>();
            CreateMap<DBInstagramAccount, InstAccountDTO>().ForMember(dest => dest.Group, opt => opt.MapFrom(src => src.InstGroup.Name));
            CreateMap<InstAccountDTO, DBInstagramAccount>();
            CreateMap<DBWorkerTask, WorkerTaskDTO>().ForMember(dest => dest.InstAccountLogin, opt => opt.MapFrom(src => src.Account.Login));

            CreateMap<WorkerTaskDTO, DBWorkerTask>();
            CreateMap<DBEmail, EmailDTO>().ReverseMap();
            CreateMap<DBSMSActivation, SMSServiceDTO>().ReverseMap();
            CreateMap<DBTask, TaskDTO>().ReverseMap();

            CreateMap<DBPost, PostListDTO>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name));
            CreateMap<PostListDTO, DBPost>();

            CreateMap<DBPost, CRUDPostDTO>().ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name));
            CreateMap<CRUDPostDTO, DBPost>();

            CreateMap<DBPostLike, LikeDTO>()
                .ForMember(dest => dest.AccountLogin, opt => opt.MapFrom(src => src.Account.Login));
            CreateMap<LikeDTO, DBPostLike>();

            CreateMap<DBPostComment, CommentDTO>()
                .ForMember(dest => dest.AccountLogin, opt => opt.MapFrom(src => src.Account.Login));
            CreateMap<CommentDTO, DBPostComment>();

            CreateMap<DBPostGroup, PostGroupDTO>()
                .ForMember(dest => dest.AccountGroupName, opt => opt.MapFrom(src => src.AccountGroup.Name))
                .ForMember(dest => dest.CountPinnedPosts, opt => opt.MapFrom(src => src.Posts.Count()));
            CreateMap<PostGroupDTO, DBPostGroup>();

            /*** BAS task Mapping ***/

            // Get task Mapping
            CreateMap<DBWorkerTask, GetRegistrationTaskEmailServiceDTO>().ForMember(dest => dest.UsefulData, opt => opt.Ignore());
            CreateMap<DBWorkerTask, GetRegistrationTaskSMSServiceDTO>().ForMember(dest => dest.UsefulData, opt => opt.Ignore());
            CreateMap<DBWorkerTask, GetAuthorizationTaskDTO>().ForMember(dest => dest.InstAccount, opt => opt.MapFrom(src => src.Account)).ReverseMap();
            CreateMap<DBWorkerTask, GetPostingTaskDTO>().ForMember(dest => dest.InstAccount, opt => opt.MapFrom(src => src.Account)).ReverseMap();
            

            // End task Mapping
            CreateMap<DBInstagramAccount, EndRegistrationTaskDTO>().ReverseMap();


        }
    }
}