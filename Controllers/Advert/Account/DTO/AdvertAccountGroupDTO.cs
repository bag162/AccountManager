namespace BASAccountManager.Controllers.Advert.Account.DTO
{
    public class AdvertAccountGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountPinnedAccounts { get; set; }
    }

    public class AddAdvertAccountGroupDTO
    {
        public string Name { get; set; }
    }

    public class DeleteAdvertAccountGroupDTO
    {
        public int Id { get; set; }
    }
}
