namespace BASAccountManager.Abstraction
{
    public abstract class Account
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime ReceiptDate { get; set; }
    }
}