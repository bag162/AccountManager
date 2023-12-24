using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Email.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface IEmailDBService
    {
        public JqueryDataTable<EmailDTO> GetEmail(int start, int lenght, string searchdata);
        public List<DBEmail> GetEmail();
        public DBEmail GetEmailById(int id);
        public Task AddEmailAsync(List<DBEmail> newEmail);

        public Task RemoveEmailAsync(List<DBEmail> removedEmail);

        public Task UpdateEmailAsync(DBEmail updatedEmail);
        public Task UpdateEmailAsync(List<DBEmail> updatedEmail);
    }
}