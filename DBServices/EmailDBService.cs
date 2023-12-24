using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Email.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;

namespace BASAccountManager.DBServices
{
    public class EmailDBService : IEmailDBService
    {
        private AMContext dbcontext;
        private ILogger<EmailDBService> logger;
        private IMapper mapper;

        public EmailDBService(AMContext amcontext, ILogger<EmailDBService> logger, IMapper mapper)
        {
            this.dbcontext = amcontext; ;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddEmailAsync(List<DBEmail> newEmail)
        {
            await this.dbcontext.Email.AddRangeAsync(newEmail);
            await this.dbcontext.SaveChangesAsync();
            return;

        }

        public JqueryDataTable<EmailDTO> GetEmail(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<EmailDTO>();
            data.recordsTotal = this.dbcontext.Email.Count();
            DBEmail[] filteredData = Array.Empty<DBEmail>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.Email.AsQueryable().Where(m => m.APIToken.Contains(searchdata)
                                                || m.MailDomain.Contains(searchdata)
                                                || m.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.Email.AsQueryable().Where(m => m.APIToken.Contains(searchdata)
                                                || m.MailDomain.Contains(searchdata)
                                                || m.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<EmailDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<EmailDTO>>(this.dbcontext.Email.AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<EmailDTO>>(this.dbcontext.Email.AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public List<DBEmail> GetEmail()
        {
            return this.dbcontext.Email.ToList();
        }

        public DBEmail GetEmailById(int id)
        {
            return this.dbcontext.Email.Find(id);
        }

        public async Task RemoveEmailAsync(List<DBEmail> removedEmail)
        {
            this.dbcontext.RemoveRange(removedEmail);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateEmailAsync(DBEmail updatedEmail)
        {
            this.dbcontext.Email.Update(updatedEmail);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task UpdateEmailAsync(List<DBEmail> updatedEmail)
        {
            this.dbcontext.Email.UpdateRange(updatedEmail);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}
