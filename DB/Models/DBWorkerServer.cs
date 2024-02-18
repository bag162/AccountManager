using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    public class DBWorkerServer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string APIKey { get; set; }

        public WorkerServerStatus WorkerServerStatus { get; set; }
    }

    public enum WorkerServerStatus
    {
        Active,
        Inactive
    }
}
// name or api key unique