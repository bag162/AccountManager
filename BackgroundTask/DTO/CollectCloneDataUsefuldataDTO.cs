namespace BASAccountManager.BackgroundTask.DTO
{
    public class CollectCloneDataUsefuldataDTO
    {
        public int ClonId { get; set; }
        public string ClonURI { get; set; }
        public int CountCommentToCollect { get; set; }
        public int CountPostToCollect { get; set; }
    }
}
