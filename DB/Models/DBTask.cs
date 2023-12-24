using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    public class DBTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ClientTaskName { get; set; }
        public StatusTask Status { get; set; }
        public TaskType TaskType { get; set; }
        public string UsefulData { get; set; }
        public string AccountGroup { get; set; }
        public string ProxyGroup { get; set; }
    }

    public enum StatusTask
    {
        Added,
        AddingProcess,
        Performed,
        Completed,
        Canceled
    }

    public enum TaskType
    { 
        RegistrationAccounts
    }
}