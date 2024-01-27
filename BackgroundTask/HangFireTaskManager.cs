using BASAccountManager.BackgroundTask;

namespace BASAccountManager
{
    public class HangFireTaskManager
    {
        private AssignmentWriter AssignmentWriter { get; set; }
        private StatusMonitor StatusMonitor { get; set; }
        public HangFireTaskManager(AssignmentWriter AssignmentWriter, StatusMonitor StatusMonitor)
        {
            this.AssignmentWriter = AssignmentWriter;
            this.StatusMonitor = StatusMonitor;
        }

        public async Task PostParser()
        {
            await this.AssignmentWriter.PostParserAsync();
        }

        public async Task TaskParser()
        {
            await this.AssignmentWriter.TaskParserAsync();
        }

        public async Task CommentParser()
        {
            await this.AssignmentWriter.CommentParserAsync();
        }

        public async Task LikesParser()
        {
            await this.AssignmentWriter.LikesParserAsync();
        }

        public async Task MonitorStatus()
        {
            // Dont work
            /*await this.StatusMonitor.CheckProxyStatusAsync();*/

            await this.StatusMonitor.CheckTaskWorkerStatusAsync();
            await this.StatusMonitor.CheckInactiveTask();
            await this.StatusMonitor.CheckUntakenComments();
            await this.StatusMonitor.CheckUntakenLikes();
            await this.StatusMonitor.CheckUntakenFollows();
            await this.StatusMonitor.CheckAdvertUntakenLikesAndComments();
            await this.StatusMonitor.CheckAdvertUntakenFollows();
        }
    }
}