namespace BASAccountManager.Controllers.Proxy.DTO
{
    public class ProxyDTO
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Group { get; set; }
        public string? ProxyStatus { get; set; }
    }
}