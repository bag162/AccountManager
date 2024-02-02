using AutoMapper;
using BASAccountManager.BackgroundTask.DTO;
using BASAccountManager.Controllers.Advert.Account.DTO;
using BASAccountManager.Controllers.Advert.Post.DTO;
using BASAccountManager.Controllers.BASTask.DTO;
using BASAccountManager.Controllers.Email.DTO;
using BASAccountManager.Controllers.Instagram.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.Controllers.Post.Group.DTO;
using BASAccountManager.Controllers.Post.Like.DTO;
using BASAccountManager.Controllers.Post.Post.DTO;
using BASAccountManager.Controllers.ProfileFilling.DTO;
using BASAccountManager.Controllers.Proxy.DTO;
using BASAccountManager.Controllers.SchedulerTask.DTO;
using BASAccountManager.Controllers.SMS_Services.DTO;
using BASAccountManager.Controllers.Task.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.AdvertResourses;
using BASAccountManager.DB.Models.Post;
using Newtonsoft.Json;

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

            CreateMap<DBPostCommentGroup, CommentGroupDTO>()
                .ForMember(dest => dest.CountPinnedPosts, opt => opt.MapFrom(src => src.ListPost.Count()))
                .ForMember(dest => dest.CountComments, opt => opt.MapFrom(src => src.ListComment.Count()));

            CreateMap<DBPost, PostListDTO>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name));
            CreateMap<PostListDTO, DBPost>();

            CreateMap<DBPost, CRUDPostDTO>()
                .ForMember(dest => dest.PostGroupName, opt => opt.MapFrom(src => src.Group.Name))
                .ForMember(dest => dest.CommentGroupName, opt => opt.MapFrom(src => src.PostCommentGroup.Name));

            CreateMap<CRUDPostDTO, DBPost>();

            CreateMap<DBComment, CommentDTO>()
                .ForMember(dest => dest.CommentGroupName, opt => opt.MapFrom(src => src.PostCommentGroup.Name));
            CreateMap<CommentDTO, DBComment>();
            CreateMap<CRUDCommentDTO, DBComment>();

            CreateMap<DBFillingData, ProfileFillingTableDTO>()
                .ForMember(dest => dest.CountLinkedAccounts, opt => opt.MapFrom(src => src.ListAccounts.Count()));
            CreateMap<ProfileFillingTableDTO, DBFillingData>();
            CreateMap<DBFillingData, GetProfileFillingDTO>();
            
            CreateMap<AddFillingDataDTO, DBFillingData>();
            CreateMap<DBFillingData, ProfileFillingUsefulDataDTO>();
            
            CreateMap<CRUDCommentGroupDTO, DBPostCommentGroup>();
            
            CreateMap<DBPostGroup, PostGroupDTO>()
                .ForMember(dest => dest.AccountGroupName, opt => opt.MapFrom(src => src.AccountGroup.Name))
                .ForMember(dest => dest.CountPinnedPosts, opt => opt.MapFrom(src => src.ListPost.Count()));
            CreateMap<PostGroupDTO, DBPostGroup>();


            CreateMap<DBPostComment, CommentUsefulDataDTO>()
                .ForMember(dest => dest.PostCommentMessage, opt => opt.MapFrom(src => src.Comment.Message))
                .ForMember(dest => dest.PostCommentURI, opt => opt.MapFrom(src => src.Post.PostURI))
                .ForMember(dest => dest.PostCommentId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<DBPostLikes, LikeUsefulDataDTO>()
                .ForMember(dest => dest.PostLikeId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PostLikeURI, opt => opt.MapFrom(src => src.Post.PostURI))
                .ReverseMap();
            CreateMap<DBFollow, FollowUsefulDataDTO>()
                .ForMember(dest => dest.FollowId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FollowURI, opt => opt.MapFrom(src => src.RecipientAccount.ProfileLink))
                .ReverseMap();

            CreateMap<DBAdvertAccount, AdvertAccountDTO>()
                .ForMember(dest => dest.AdvertAccountGroupName, opt => opt.MapFrom(src => src.AdvertAccountGroup.Name));
            CreateMap<DBAdvertAccountGroup, AdvertAccountGroupDTO>()
                .ForMember(dest => dest.CountPinnedAccounts, opt => opt.MapFrom(src => src.ListAdvertAccount.Count()));

            CreateMap<DBAdvertPost, AdvertPostDTO>()
                .ForMember(dest => dest.AdvertPostGroupName, opt => opt.MapFrom(src => src.AdvertPostGroup.Name));
            CreateMap<DBAdvertPostGroup, AdvertPostGroupDTO>()
                .ForMember(dest => dest.CountPinnedPosts, opt => opt.MapFrom(src => src.ListAdvertPost.Count()));
            CreateMap<DBSchedulerTask, SchedulerTaskDTO>()
                .ForMember(dest => dest.CountPinnedTasks, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<int[]>(src.TaskIds).Count()));

            /*** BAS task Mapping ***/

            // Get task Mapping
            CreateMap<DBWorkerTask, GetRegistrationTaskEmailServiceDTO>().ForMember(dest => dest.UsefulData, opt => opt.Ignore());
            CreateMap<DBWorkerTask, GetRegistrationTaskSMSServiceDTO>().ForMember(dest => dest.UsefulData, opt => opt.Ignore());
            CreateMap<DBWorkerTask, GetAuthorizationTaskDTO>().ForMember(dest => dest.InstAccount, opt => opt.MapFrom(src => src.Account)).ReverseMap();
            CreateMap<DBWorkerTask, GetPostingTaskDTO>().ForMember(dest => dest.InstAccount, opt => opt.MapFrom(src => src.Account)).ReverseMap();
            CreateMap<DBWorkerTask, GetCommentingTaskDTO>()
                .ForMember(dest => dest.InstAccount, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<DBWorkerTask, GetLikingTaskDTO>()
                .ForMember(dest => dest.InstAccount, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.Likes, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<DBWorkerTask, GetFollowingTaskDTO>()
                .ForMember(dest => dest.InstAccount, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.Follows, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<DBWorkerTask, GetProfileFillingTask>()
                .ForMember(dest => dest.InstAccount, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.ProfileFillingData, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<DBWorkerTask, GetAdvertFollowingTaskDTO>();
            CreateMap<DBWorkerTask, GetAdvertLikingTaskDTO>();
            CreateMap<DBWorkerTask, GetAdvertCommentingTaskDTO>();

            // End task Mapping
            CreateMap<DBInstagramAccount, EndRegistrationTaskDTO>().ReverseMap();


        }
    }
}