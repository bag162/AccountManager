using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.SMS_Services.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;

namespace BASAccountManager.DBServices
{
    public class SMSDBService : ISMSServiceDB
    {
        private AMContext dbcontext;
        private ILogger<SMSDBService> logger;
        private IMapper mapper;

        public SMSDBService(AMContext amcontext, ILogger<SMSDBService> logger, IMapper mapper)
        {
            this.dbcontext = amcontext; ;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddSMSServiceAsync(List<DBSMSActivation> newSMSService)
        {
            await this.dbcontext.SMSActivation.AddRangeAsync(newSMSService.ToArray());
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public List<DBSMSActivation> GetSMSService()
        {
            return this.dbcontext.SMSActivation.ToList();
        }

        public JqueryDataTable<SMSServiceDTO> GetSMSService(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<SMSServiceDTO>();
            data.recordsTotal = this.dbcontext.SMSActivation.Count();
            DBSMSActivation[] filteredData = Array.Empty<DBSMSActivation>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.SMSActivation.AsQueryable().Where(m => m.ServiceName.Contains(searchdata)
                                                || m.APIURI.Contains(searchdata)
                                                || m.APIKey.Contains(searchdata)
                                                || m.Id.Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.SMSActivation.AsQueryable().Where(m => m.ServiceName.Contains(searchdata)
                                                || m.APIURI.Contains(searchdata)
                                                || m.APIKey.Contains(searchdata)
                                                || m.Id.Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<SMSServiceDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<SMSServiceDTO>>(this.dbcontext.SMSActivation.AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<SMSServiceDTO>>(this.dbcontext.SMSActivation.AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public DBSMSActivation GetSMSServiceById(int id)
        {
            return this.dbcontext.SMSActivation.Where(x => x.Id == id).First();
        }

        public async Task RemoveSMSServiceAsync(List<DBSMSActivation> removedSMSService)
        {
            this.dbcontext.SMSActivation.RemoveRange(removedSMSService);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateSMSServiceAsync(List<DBSMSActivation> updatedSMSService)
        {
            this.dbcontext.SMSActivation.UpdateRange(updatedSMSService);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}
