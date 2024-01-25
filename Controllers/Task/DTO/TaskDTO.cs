using BASAccountManager.DB.Models;
using Newtonsoft.Json;

namespace BASAccountManager.Controllers.Task.DTO
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string ClientTaskName { get; set; }
        public string Status { get; set; }
        public string TaskType { get; set; }
        public string? UsefulData { get; set; }
        public string AccountGroup { get; set; }
        public string ProxyGroup { get; set; }
    }

    public class RegistrationTaskWorkerUsefulDataDTO
    {
        public int CountAccount { get; set; }

        public RegistrationVerifyResoursesType RegistrationVerifyResoursesType { get; set; }
        public int? SMSServiceId { get; set; }
        public int? EmailServiceId { get; set; }
    }

    public class PostingTaskWorkerUsefilDataDTO
    {
        public int PostPerAccount { get; set; }
    }

    public class CommentingTaskWorkerUsefulDataDTO
    {
        public string PostGroup { get; set; }
        public int CommentsPerAccount { get; set; }
    }

    public class LikingTaskWorkerUsefulDatadTO
    {
        public string PostGroup { get; set; }
        public int LikesPerAccount { get; set; }
    }

    public class FollowingTaskWorkerUsefulDataDTO
    {
        public string AccountGroupForSubscription { get; set; }
        public int FollowsPerAccount { get; set; }
        public int RequiredFollowersPerAccount { get; set; }
    }

    public class FillingProfileTaskWorkerUsefulDataDTO
    {
        public string FillingProfileName { get; set; }
    }

    public class AdvertLikingTaskWorkerUsefulDatadTO
    {
        public string AdvertPostGroup { get; set; }
        public int LikesPerAccount { get; set; }
        public bool LikeIfPostCommentedPreviously { get; set; }
    }

    public class AdvertCommentingTaskWorkerUsefulDatadTO
    {
        public string AdvertPostGroup { get; set; }
        public string CommentsGroup { get; set; }
        public int CommentsPerAccount { get; set; }
        public bool CommentIfPostLikedPreviously { get; set; }
    }

    public class AdvertFollowingTaskWorkerUsefulDatadTO
    {
        public string AdvertAccountGroup { get; set; }
        public int FollowsPerAccount { get; set; }
    }
}