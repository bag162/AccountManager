using BASAccountManager.DB.Models.AdvertResourses;

namespace BASAccountManager.Controllers.Advert.Account.DTO
{
    public class AdvertAccountDTO
    {
        public int Id { get; set; }
        public string AccountURL { get; set; }
        public string AdvertAccountStatus { get; set; }
        public string AdvertAccountGroupName { get; set; }
    }

    public class AddAdvertAccountDTO
    {
        public string AccountURL { get; set; }
        public string AdvertAccountGroupName { get; set; }
    }

    public class DeleteAdvertAccountDTO
    {
        public int Id { get; set; }
    }
}