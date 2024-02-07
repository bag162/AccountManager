using BASAccountManager.BackgroundTask.DTO;
using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.AdvertResourses;

namespace BASAccountManager.Controllers.BASTask.DTO
{
    public class GetAdvertLikingTaskDTO
    {
        public int Id { get; set; }
        public DBInstagramAccount Account { get; set; }
        public DBProxy Proxy { get; set; }
        public List<DBAdvertPost> AdvertPosts { get; set; }
        public string TaskType { get; set; }
    }

    public class GetAdvertCommentingTaskDTO
    {
        public int Id { get; set; }
        public DBInstagramAccount Account { get; set; }
        public DBProxy Proxy { get; set; }
        public List<DBAdvertPost> AdvertPosts { get; set; }
        public string TaskType { get; set; }
    }

    public class GetAdvertFollowingTaskDTO
    {
        public int Id { get; set; }
        public DBInstagramAccount Account { get; set; }
        public DBProxy Proxy { get; set; }
        public List<DBAdvertAccount> AdvertAccounts { get; set; }
        public string TaskType { get; set; }
    }

    public class EndIntermediateAdvertLikingTaskDTO
    {
        public int WorkerId { get; set; }
        public int AdvertPostId { get; set; }
    }

    public class EndIntermediateAdvertCommentingTaskDTO
    {
        public int WorkerId { get; set; }
        public int AdvertPostId { get; set; }
    }

    public class EndIntermediateAdvertFollowingTaskDTO
    {
        public int WorkerId { get; set; }
        public int AdvertAccountId { get; set; }
    }

    public class EndAdvertLikingTaskDTO
    {
        public int WorkerId { get; set; }
    }

    public class EndAdvertCommentingTaskDTO
    {
        public int WorkerId { get; set; }
    }

    public class EndAdvertFollowingTaskDTO
    {
        public int WorkerId { get; set; }
    }

    public class GetCollectCloningDataTaskDTO
    {
        public int Id { get; set; }
        public DBInstagramAccount Account { get; set; }
        public DBProxy Proxy { get; set; }
        public CollectCloneDataUsefuldataDTO CollectData { get; set; }
        public string TaskType { get; set; }
    }

    public class EndIntermediateParseCloningInformationProfileData
    {
        public int WorkerId { get; set; }
        public int ClonId { get; set; }
        public string ImageBase64Data { get; set; }
        public string ImageFormat { get; set; }
        public string ProfileDescription { get; set; }
    }

    public class EndIntermediateParseCloningInformationPostData
    {
        public int WorkerId { get; set; }
        public int ClonId { get; set; }
        public string ImageBase64Data { get; set; }
        public string ImageFormat { get; set; }
        public string Description { get; set; }
        public string[] Comments { get; set; }
        public string PostURI { get; set; }
    }

    public class EndIntermediateParseCloningInformationData
    {
        public int WorkerId { get; set; }
        public int ClonId { get; set; }
    }

}