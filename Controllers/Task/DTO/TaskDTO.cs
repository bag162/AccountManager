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
        public string? AccountGroup { get; set; }
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
        public string ResourseType { get; set; }
        public string? ClonGroupName { get; set; }
    }

    public class CommentingTaskWorkerUsefulDataDTO
    {
        public string PostGroup { get; set; }
        public int CommentsPerAccount { get; set; }

        public string commentingAccountResourses { get; set; }
        public string commentingPostResourses { get; set; }

        public string? commentingAccountClonName { get; set; }
        public string? commentingPostClonName { get; set; }
    }

    public class LikingTaskWorkerUsefulDatadTO
    {
        public string PostGroup { get; set; }
        public int LikesPerAccount { get; set; }

        public string ResourseLikingType { get; set; }
        public string? LikingClonGroupName { get; set; }

        public string ResourseAccountType { get; set; }
        public string? AccountClonGroupName { get; set; }

    }

    public class FollowingTaskWorkerUsefulDataDTO
    {
        public string AccountGroupForSubscription { get; set; }
        public int FollowsPerAccount { get; set; }
        public int RequiredFollowersPerAccount { get; set; }

        public string followingResourseType { get; set; }
        public string? followingClonName { get; set; }

        public string accountResourseType { get; set; }
        public string? accountClonName { get; set; }
    }

    public class FillingProfileTaskWorkerUsefulDataDTO
    {
        public string? FillingProfileName { get; set; }
        public string profileFillingResourse { get; set; }
        public string? clonName { get; set; }
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

    public class ParseCloningInformationTaskWorkerUsefuldataDTO
    {
        public string CloneGroupForSave { get; set; }
        public string AdvertAccountGroupsForCloning { get; set; }
        public int CountCommentToCollect { get; set; }
        public int CountPostToCollect { get; set; }

        public string? NameOrSurnameGenString { get; set; }
        public string? UsernameGenString { get; set; }
    }
}