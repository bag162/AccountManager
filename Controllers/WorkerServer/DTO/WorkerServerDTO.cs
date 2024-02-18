namespace BASAccountManager.Controllers.WorkerServer.DTO
{
    public class GetWorkerServerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string APIKey { get; set; }
        public string WorkerServerStatus { get; set; }
    }

    public class AddWorkerServerDTO
    {
        public string Name { get; set; }
        public string APIKey { get; set; }
        public string WorkerServerStatus { get; set; }
    }

    public class ChangeStatusDTO
    {
        public int? Id { get; set; }
    }
}