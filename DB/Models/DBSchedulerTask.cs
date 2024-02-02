using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BASAccountManager.DB.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class DBSchedulerTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string TaskIds { get; set; }
        public int TimeBetweenLaunchesMinutes { get; set; }

        public int? CurrentTaskPositionIndex { get; set; }
        public DateTime? LastStart { get; set; }

        public StartupType StartupType { get; set; }
        public SchedulerTaskStatus SchedulerTaskStatus { get; set; }
    }
}

public enum StartupType
{
    Repeating
}

public enum SchedulerTaskStatus
{
    Started,
    Stopped,
    Completed
}