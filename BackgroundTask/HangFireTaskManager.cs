using BASAccountManager.BackgroundTask;
using Microsoft.Extensions.Caching.Memory;

namespace BASAccountManager
{
    public class HangFireTaskManager
    {
        private AssignmentWriter AssignmentWriter { get; set; }
        private StatusMonitor StatusMonitor { get; set; }
        private IMemoryCache cache;

        public HangFireTaskManager(AssignmentWriter AssignmentWriter, StatusMonitor StatusMonitor, IMemoryCache cache)
        {
            this.AssignmentWriter = AssignmentWriter;
            this.StatusMonitor = StatusMonitor;
            this.cache = cache;
        }

        public async Task ParseSchedulerTask()
        {
            this.cache.TryGetValue("ParseSchedulerTask", out bool? isActive);
            if (isActive == true)
            {
                return;
            }
            else
            {
                this.cache.Set("ParseSchedulerTask", true, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }

            await this.AssignmentWriter.ParseSchedulerTask();
            this.cache.Set("ParseSchedulerTask", false);
        }

        public async Task PostParser()
        {
            this.cache.TryGetValue("PostParserTaskStatus", out bool? isActive);
            if (isActive == true)
            {
                return;
            }
            else
            {
                this.cache.Set("PostParserTaskStatus", true, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }

            await this.AssignmentWriter.PostParserAsync();
            this.cache.Set("PostParserTaskStatus", false);
        }

        public async Task TaskParser()
        {
            this.cache.TryGetValue("TaskParserTaskStatus", out bool? isActive);
            if (isActive == true)
            {
                return;
            }
            else
            {
                this.cache.Set("TaskParserTaskStatus", true, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }
            await this.AssignmentWriter.TaskParserAsync();
            await this.StatusMonitor.CheckProxyStatusAsync();
            this.cache.Set("TaskParserTaskStatus", false);
        }

        public async Task CommentParser()
        {
            this.cache.TryGetValue("CommentParserTaskStatus", out bool? isActive);
            if (isActive == true)
            {
                return;
            }
            else
            {
                this.cache.Set("CommentParserTaskStatus", true, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }
            await this.AssignmentWriter.CommentParserAsync();
            this.cache.Set("CommentParserTaskStatus", false);
        }

        public async Task LikesParser()
        {
            this.cache.TryGetValue("LikesParserTaskStatus", out bool? isActive);
            if (isActive == true)
            {
                return;
            }
            else
            {
                this.cache.Set("LikesParserTaskStatus", true, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }
            await this.AssignmentWriter.LikesParserAsync();
            this.cache.Set("LikesParserTaskStatus", false);
        }

        public async Task MonitorStatus()
        {
            this.cache.TryGetValue("MonitorStatusTaskStatus", out bool? isActive);
            if (isActive == true)
            {
                return;
            }
            else
            {
                this.cache.Set("MonitorStatusTaskStatus", true, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
            }

            await this.StatusMonitor.CheckTaskWorkerStatusAsync();
            await this.StatusMonitor.CheckInactiveTask();
            await this.StatusMonitor.CheckUntakenComments();
            await this.StatusMonitor.CheckUntakenLikes();
            await this.StatusMonitor.CheckUntakenFollows();
            await this.StatusMonitor.CheckAdvertUntakenLikesAndComments();
            await this.StatusMonitor.CheckAdvertUntakenFollows();
            await this.StatusMonitor.CheckUntakenClon();
            await this.StatusMonitor.CheckWorkerTaskError();
            await this.StatusMonitor.CheckPostStatus();

            this.cache.Set("MonitorStatusTaskStatus", false);
        }
    }
}