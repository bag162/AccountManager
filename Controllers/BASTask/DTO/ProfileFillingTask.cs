using BASAccountManager.BackgroundTask.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.Controllers.BASTask.DTO
{
    public class EndProfileFillingTask
    {
        public int WorkerId { get; set; }
        public string NewUsername { get; set; }
        public string NewName { get; set; }
        public string NewSurname { get; set; }
    }

    public class GetProfileFillingTask
    {
        public int Id { get; set; }
        public DBInstagramAccount InstAccount { get; set; }
        public DBProxy Proxy { get; set; }
        public ProfileFillingUsefulDataDTO ProfileFillingData { get; set; }
        public string TaskType { get; set; }
    }
}