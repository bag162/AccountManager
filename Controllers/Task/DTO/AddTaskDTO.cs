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
        public string ResourseType { get; set; }
        public string? ClonGroupName { get; set; }
    }

    public class CommentingTaskDTO
    {
        public string? ClientTaskName { get; set; }
        public string ProxyGroup { get; set; }
        public string? AccountGroup { get; set; }
        public string? PostGroup { get; set; }
        public int CommentsPerAccount { get; set; }

        public string commentingAccountResourses { get; set; }
        public string commentingPostResourses { get; set; }

        public string? commentingAccountClonName { get; set; }
        public string? commentingPostClonName { get; set; }
    } 

    public class LikingTaskDTO
    {
        public string? ClientTaskName { get; set; }
        public string? PostGroup { get; set; }

       
        public string ProxyGroup { get; set; }
        public string? AccountGroup { get; set; }
        public int LikesPerAccount { get; set; }

        public string ResourseLikingType { get; set; }
        public string? LikingClonGroupName { get; set; }

        public string ResourseAccountType { get; set; }
        public string? AccountClonGroupName { get; set; }
    }

    public class FollowingTaskDTO
    {
        public string? ClientTaskName { get; set; }
        public string ProxyGroup { get; set; }
        public string? AccountGroupForSubscription { get; set; }
        public string? AccountGroup { get; set; }
        public int FollowsPerAccount { get; set; }
        public int RequiredFollowersPerAccount { get; set; }

        public string followingResourseType { get; set; }
        public string? followingClonName { get; set; }

        public string accountResourseType { get; set; }
        public string? accountClonName { get; set; }
    }

    public class FillingProfileTaskDTO
    {
        public string? ClientTaskName { get; set; }
        
        public string ProxyGroup { get; set; }
        public string AccountGroup { get; set; }
        public string? FillingProfileName { get; set; }

        public string profileFillingResourse { get; set; }
        public string? clonName { get; set; }
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

    public class ParseCloningInformation
    {
        public string TaskName { get; set; }
        public string AccountGroup { get; set; }
        public string ProxyGroup { get; set; }
        public string CloneGroupForSave { get; set; }
        public string AdvertAccountGroupsForCloning { get; set; }
        public int CountCommentToCollect { get; set; }
        public int CountPostToCollect { get; set; }

        public string? NameOrSurnameGenString { get; set; }
        public string? UsernameGenString { get; set; }
    }
}