using AutoMapper;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices
{
    public class ClonDBService : IClonDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;

        public ClonDBService(AMContext dbcontext,
            IMapper mapper)
        {
            this.dbcontext = dbcontext;
            this.mapper = mapper;
        }

        public async Task<int> AddCloneAsync(DBClon data)
        {
            this.dbcontext.Clon.Add(data);
            await this.dbcontext.SaveChangesAsync();
            return this.dbcontext.Clon.Where(x => x.ClonURI == data.ClonURI).First().Id;
        }

        public async Task DeleteClonesAsync(List<DBClon> data)
        {
            this.dbcontext.Clon.RemoveRange(data);
            await this.dbcontext.SaveChangesAsync();
        }

        public DBClon GetClonById(int id)
        {
            return this.dbcontext.Clon.Include(x => x.ClonGroup).Include(x => x.PostGroup).Where(x => x.Id == id).First();
        }

        public List<DBClon> GetClones()
        {
            return this.dbcontext.Clon.Include(x => x.PostGroup).ThenInclude(x => x.ListPost).ThenInclude(x => x.PostCommentGroup).Include(x => x.FillingData).ToList();
        }

        public async Task UpdateCloneAsync(DBClon data)
        {
            this.dbcontext.Clon.Update(data);
            await this.dbcontext.SaveChangesAsync();
        }
    }
}