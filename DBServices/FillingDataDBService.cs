using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Email.DTO;
using BASAccountManager.Controllers.ProfileFilling.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using BASAccountManager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BASAccountManager.DBServices
{
    public class FillingDataDBService : IFillingDataDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;

        public FillingDataDBService(AMContext amcontext, IMapper mapper)
        {
            this.dbcontext = amcontext;
            this.mapper = mapper;
        }

        public async Task AddFillingDataAsync(AddFillingDataDTO data)
        {
            var fillingData = mapper.Map<DBFillingData>(data);
            var path = "wwwroot/images/avatars/" + data.Name;
            string filePath = await ImageService.AddBase64ImageAsync(data.AvatarFormat, data.AvatarBASE64, path, data.Name);
            fillingData.AvatarPath = filePath.Remove(0, 8);
            await this.dbcontext.FillingData.AddAsync(fillingData);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task DeleteAsync(List<DBFillingData> data)
        {
            this.dbcontext.FillingData.RemoveRange(data);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public DBFillingData GetById(int id)
        {
            return this.dbcontext.FillingData.Find(id);
        }

        public JqueryDataTable<ProfileFillingTableDTO> GetFillingData(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<ProfileFillingTableDTO>();
            data.recordsTotal = this.dbcontext.Email.Count();
            DBFillingData[] filteredData = Array.Empty<DBFillingData>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.FillingData.AsQueryable().Include(x => x.ListAccounts).Where(m =>  m.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.FillingData.AsQueryable().Include(x => x.ListAccounts).Where(m => m.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<ProfileFillingTableDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<ProfileFillingTableDTO>>(this.dbcontext.FillingData.AsQueryable().Include(x => x.ListAccounts).Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<ProfileFillingTableDTO>>(this.dbcontext.FillingData.AsQueryable().Include(x => x.ListAccounts).Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public DBFillingData GetFillingDataByName(string name)
        {
            return this.dbcontext.FillingData.Where(x => x.Name == name).First();
        }

        public List<string> GetFillingDataNames()
        {
            return this.dbcontext.FillingData.AsQueryable().Select(x => x.Name).ToList();
        }
    }
}