namespace BASAccountManager.Controllers.Clon.DTO
{
    public class AddClonGroupDTO
    {
        public string Name { get; set; }
    }

    public class GetClonGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountPinnedClones { get; set; }
    }
}
