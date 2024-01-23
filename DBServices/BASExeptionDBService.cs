using AutoMapper;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;

namespace BASAccountManager.DBServices
{
    public class BASExeptionDBService : IBASExeptionDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;

        public BASExeptionDBService(AMContext amcontext, IMapper mapper)
        {
            this.dbcontext = amcontext;
            this.mapper = mapper;
        }

        public async Task AddAsync(DBBASExeption exeption)
        {
            this.dbcontext.BASExeption.Add(exeption);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
    }
}
