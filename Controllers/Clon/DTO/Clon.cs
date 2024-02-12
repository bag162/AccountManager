
namespace BASAccountManager.Controllers.Clon.DTO
{
    public class GetClonDTO
    {
        public int Id { get; set; }
        public string ClonStatus { get; set; }
        public string GroupName { get; set; }
        public int CountPinnedAccount { get; set; }
        public int CountPinnedPosts { get; set; }
    }

    public class ClonData
    {
        public int Id { get; set; }
        public string ClonURI { get; set; }
        public string ClonStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string GroupName { get; set; }
        public int FillingDataId { get; set; }
    }
}