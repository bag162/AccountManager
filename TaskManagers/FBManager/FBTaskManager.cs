namespace BASAccountManager.TaskManagers.FBManager
{
    public class FBTaskManager
    {
        public ILogger<FBTaskManager> Logger { get; set; }

        public FBTaskManager(ILogger<FBTaskManager> logger)
        {
            this.Logger = logger;
        }

        public void AddTask()
        {

        }

        public void GetTask()
        {

        }
    }
}