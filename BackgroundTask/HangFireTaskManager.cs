using BASAccountManager.BackgroundTask;

namespace BASAccountManager
{
    public class HangFireTaskManager
    {
        private AssignmentWriter AssignmentWriter { get; set; }

        public HangFireTaskManager(AssignmentWriter AssignmentWriter)
        {
            this.AssignmentWriter = AssignmentWriter;
        }

        public async Task TaskParser()
        {
            await this.AssignmentWriter.TaskParser();
        }
    }
}