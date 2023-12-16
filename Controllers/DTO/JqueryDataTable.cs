namespace BASAccountManager.Controllers.DTO
{
    public class JqueryDataTable<T>
    {
        public List<T> data { get; set; }
        public int draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
    }
}