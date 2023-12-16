using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.SMS_Services.DTO;
using BASAccountManager.DB.Models;

namespace BASAccountManager.DBServices.Interfaces
{
    public interface ISMSServiceDB
    {
        public List<DBSMSActivation> GetSMSService();
        public JqueryDataTable<SMSServiceDTO> GetSMSService(int start, int lenght, string searchdata);
        public Task AddSMSServiceAsync(List<DBSMSActivation> newProxy);
        public Task RemoveSMSServiceAsync(List<DBSMSActivation> removedProxy);
        public Task UpdateSMSServiceAsync(List<DBSMSActivation> updatedProxy);
    }
}