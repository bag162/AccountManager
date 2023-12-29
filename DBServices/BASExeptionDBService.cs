using AutoMapper;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;

namespace BASAccountManager.DBServices
{
    public class BASExeptionDBService : IBASExeptionDBService
    {
        private AMContext dbcontext;
        private ILogger<BASExeptionDBService> logger;
        private IMapper mapper;

        public BASExeptionDBService(AMContext amcontext, ILogger<BASExeptionDBService> logger, IMapper mapper)
        {
            this.dbcontext = amcontext; ;
            this.logger = logger;
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
