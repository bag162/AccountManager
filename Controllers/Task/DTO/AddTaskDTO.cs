using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.Task.DTO
{
    public class RegistrationTaskDTO
    {
        public string? ClientTaskName { get; set; }
        public string ProxyGroup { get; set; }
        public string AccountGroup { get; set; }
        public int CountAccount { get; set; }

        public RegistrationVerifyResoursesType ResoursesType { get; set; }
        public int? SMSServiceId { get; set; }
        public int? EmailServiceId { get; set; }
    }

    public class AuthorizationTaskDTO
    {
        public string? ClientTaskName { get; set; }
        public string ProxyGroup { get; set; }
        public string AccountGroup { get; set; }
    }

    public class PostingTaskDTO
    {
        public string? ClientTaskName { get; set; }
        public string ProxyGroup { get; set; }
        public string AccountGroup { get; set; }
        public int PostPerAccount { get; set; }
    }

    public class CommentingTaskDTO
    {
        public string? ClientTaskName { get; set; }
        public string ProxyGroup { get; set; }
        public string AccountGroup { get; set; }
        public string PostGroup { get; set; }
        public int CommentsPerAccount { get; set; }
    } 

    public class LikingTaskDTO
    {
        public string? ClientTaskName { get; set; }
        public string ProxyGroup { get; set; }
        public string AccountGroup { get; set; }
        public string PostGroup { get; set; }
        public int LikesPerAccount { get; set; }
    }

    public class FollowingTaskDTO
    {
        public string? ClientTaskName { get; set; }
        public string ProxyGroup { get; set; }
        public string AccountGroupForSubscription { get; set; }
        public string AccountGroup { get; set; }
        public int FollowsPerAccount { get; set; }
        public int RequiredFollowersPerAccount { get; set; }
    }

    public class FillingProfileTaskDTO
    {
        public string? ClientTaskName { get; set; }
        
        public string ProxyGroup { get; set; }
        public string AccountGroup { get; set; }
        public string FillingProfileName { get; set; }
    }

    public class AdvertCommentingTaskDTO
    {
        public string TaskName { get; set; }
        public string AccountGroup { get; set; }
        public string ProxyGroup { get; set; }
        public string AdvertPostGroup { get; set; }
        public string CommentsGroup { get; set; }
        public int CommentsPerAccount { get; set; }
        public bool CommentIfPostLikedPreviously { get; set; }
    }

    public class AdvertLikingTaskDTO
    {
        public string TaskName { get; set; }
        public string AccountGroup { get; set; }
        public string ProxyGroup { get; set; }
        public string AdvertPostGroup { get; set; }
        public int LikesPerAccount { get; set; }
        public bool LikeIfPostCommentedPreviously { get; set; }
    }

    public class AdvertFollowingTaskDTO
    {
        public string TaskName { get; set; }
        public string AccountGroup { get; set; }
        public string ProxyGroup { get; set; }
        public string AdvertAccountGroup { get; set; }
        public int FollowsPerAccount { get; set; }
    }
}