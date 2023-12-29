using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    public class DBWorkerTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? WorkerId { get; set; }
        public string? InstanceId { get; set; }
        public TaskStatus Status { get; set; }

        [ForeignKey(nameof(DBInstagramAccount))]
        public int InstAccountId { get; set; }
        public DBInstagramAccount InstAccount { get; set; }

        [ForeignKey(nameof(DBProxy))]
        public int ProxyId { get; set; }
        public DBProxy Proxy { get; set; }

        [ForeignKey(nameof(DBTask))]
        public int DBTaskId { get; set; }
        public DBTask Task { get; set; }

        public string? UsefulData { get; set; }
        public string? ErrorMessage { get; set; }
        public TaskType TaskType { get; set; }
        public RegistrationVerifyResoursesType? RegistrationVerifyResoursesType { get; set; }
    }

    public enum TaskStatus
    {
        NotTaken,
        AtWork,
        Completed,
        Error
    }

    public enum RegistrationVerifyResoursesType
    {
        EmailService,
        SMSService
    }
}