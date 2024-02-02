namespace BASAccountManager.Controllers.SchedulerTask.DTO
{
    public class SchedulerTaskDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountPinnedTasks { get; set; }
        public string SchedulerTaskStatus { get; set; }
    }

    public class AddSchedulerTaskDTO
    {
        public int? Id { get; set; }
        public string StartupType { get; set; }
        public int TimeBetweenLaunchesMinutes { get; set; }
        public string SchedulerTaskName { get; set; }
        public int[] TaskIds { get; set; }
    }

    public class UpdateSchedulerTaskDTO
    {
        public int Id { get; set; }
        public int TimeBetweenLaunchesMinutes { get; set; }
        public int[] TaskIds { get; set; }
    }
}