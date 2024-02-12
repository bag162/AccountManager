using AutoMapper;
using BASAccountManager.Controllers.Clon.DTO;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.Interfaces;
using BASAccountManager.DBServices.PostDBServices;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices
{
    public class ClonDBService : IClonDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;
        private IPostCommentGroupDBService postCommentGroupDBService;
        private IFillingDataDBService fillingDataDBService;
        private IPostGroupDBService postGroupDBService;

        public ClonDBService(AMContext dbcontext,
            IMapper mapper,
            IPostCommentGroupDBService postCommentGroupDBService,
            IFillingDataDBService fillingDataDBService,
            IPostGroupDBService postGroupDBService)
        {
            this.dbcontext = dbcontext;
            this.mapper = mapper;
            this.postCommentGroupDBService = postCommentGroupDBService;
            this.postGroupDBService = postGroupDBService;
            this.fillingDataDBService = fillingDataDBService;
        }

        public async Task<int> AddCloneAsync(DBClon data)
        {
            this.dbcontext.Clon.Add(data);
            await this.dbcontext.SaveChangesAsync();
            return this.dbcontext.Clon.Where(x => x.ClonURI == data.ClonURI).First().Id;
        }

        public async Task DeleteClonesAsync(List<DBClon> data)
        {
            // Подгрузка данных для удаления
            foreach (var item in data)
            {
                if (item.FillingDataId != null)
                {
                    await this.dbcontext.Entry(item)
                        .Reference(x => x.FillingData)
                        .LoadAsync();
                }

                await this.dbcontext.Entry(item)
                    .Collection(x => x.ListInstAccount)
                    .LoadAsync();

                if (item.PostGroupId != null)
                {
                    await this.dbcontext.Entry(item)
                        .Reference(x => x.PostGroup)
                        .LoadAsync();

                    await this.dbcontext.Entry(item.PostGroup)
                        .Collection(x => x.ListPost)
                        .LoadAsync();

                    foreach (var item1 in item.PostGroup.ListPost)
                    {
                        await this.dbcontext.Entry(item1)
                            .Reference(x => x.PostCommentGroup)
                            .LoadAsync();
                    }
                }
                
            }
            var deletedCommentGroup = new List<DBPostCommentGroup>();
            var deletedPostGroup = new List<DBPostGroup>();
            var deletedFillingData = new List<DBFillingData>();

            foreach (var item in data)
            {
                if (item.FillingDataId != null)
                {
                    deletedFillingData.Add(item.FillingData);
                }
                if (item.PostGroupId != null)
                {
                    deletedPostGroup.Add(item.PostGroup);
                    if (item.PostGroup.ListPost.Count() != 0)
                    {
                        foreach (var item1 in item.PostGroup.ListPost)
                        {
                            deletedCommentGroup.Add(item1.PostCommentGroup);
                        }
                    }
                }

            }
            var updatedAccounts = new List<DBInstagramAccount>();
            foreach (var item in data)
            {
                if (item.ListInstAccount.Count() == 0)
                {
                    continue;
                }
                var accounts = item.ListInstAccount;
                foreach (var item1 in accounts)
                {
                    item1.ClonId = null;
                }
                updatedAccounts.AddRange(accounts);
            }

            this.dbcontext.InstAccount.UpdateRange(updatedAccounts);
            this.dbcontext.Clon.RemoveRange(data);
            await this.dbcontext.SaveChangesAsync();

            await this.postCommentGroupDBService.DeleteCommentGroupAsync(deletedCommentGroup);
            await this.fillingDataDBService.DeleteAsync(deletedFillingData);
            await this.postGroupDBService.DeleteGroupsAsync(deletedPostGroup);
        }

        public async Task DelteteClonesByIdAsync(int[] ids)
        {
            var clones = this.dbcontext.Clon.Where(x => ids.Contains(x.Id)).ToList();
            await this.DeleteClonesAsync(clones);
            return;
        }

        public DBClon GetClonById(int id)
        {
            return this.dbcontext.Clon
                .Include(x => x.ClonGroup)
                .Include(x => x.PostGroup).ThenInclude(x => x.ListPost).ThenInclude(x => x.PostCommentGroup)
                .Include(x => x.FillingData)
                .Where(x => x.Id == id).First();
        }

        public ClonData GetClonData(int clonId)
        {
            var clon = this.dbcontext.Clon.Include(x => x.ClonGroup).Where(x => x.Id == clonId).First();
            return mapper.Map<ClonData> (clon);
        }

        public List<DBClon> GetClones()
        {
            return this.dbcontext.Clon
                .Include(x => x.PostGroup).ThenInclude(x => x.ListPost).ThenInclude(x => x.PostCommentGroup)
                .Include(x => x.FillingData).ToList();
        }

        public JqueryDataTable<GetClonDTO> GetClones(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<GetClonDTO>();
            data.recordsTotal = this.dbcontext.Clon.Count();
            List<DBClon> filteredData = new List<DBClon>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                filteredData = this.dbcontext.Clon.Include(x => x.ListInstAccount).Include(x => x.ClonGroup).Include(x => x.PostGroup).ThenInclude(x => x.ListPost).AsQueryable()
                                                                    .Where(m => m.ListInstAccount.Count().Equals(searchdata)
                                                                    || m.ClonGroup.Name.Contains(searchdata)
                                                                    || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToList();
                data.recordsFiltered = filteredData.Count();
                if (lenght != -1)
                {
                    filteredData = filteredData.Take(lenght).ToList();
                }

                data.data = mapper.Map<List<GetClonDTO>>(filteredData);
                
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                var listData = this.dbcontext.Clon.Include(x => x.ListInstAccount).Include(x => x.ClonGroup).Include(x => x.PostGroup).ThenInclude(x => x.ListPost).AsQueryable().ToList();
                data.recordsFiltered = listData.Count();
                if (lenght == -1)

                {
                    listData = listData.Skip(start).Take(data.recordsTotal).ToList();
                }
                else
                {
                    listData = listData.Skip(start).Take(lenght).ToList();
                }
                data.data = mapper.Map<List<GetClonDTO>>(listData.ToArray());
                
            }
            return data;
        }

        public JqueryDataTable<GetClonDTO> GetClonesByGroup(int start, int lenght, string searchdata, int groupId)
        {
            var data = new JqueryDataTable<GetClonDTO>();
            data.recordsTotal = this.dbcontext.Clon.Count();
            List<DBClon> filteredData = new List<DBClon>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                filteredData = this.dbcontext.Clon.Include(x => x.ListInstAccount).Include(x => x.PostGroup).ThenInclude(x => x.ListPost).Include(x => x.ClonGroup).AsQueryable()
                                                                    .Where(x => x.ClonGroupId == groupId)
                                                                    .Where(m => m.ListInstAccount.Count().Equals(searchdata)
                                                                    || m.ClonGroup.Name.Contains(searchdata)
                                                                    || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToList();

                if (lenght != -1)
                {
                    filteredData = filteredData.Take(lenght).ToList();
                }

                data.data = mapper.Map<List<GetClonDTO>>(filteredData);
                data.recordsFiltered = filteredData.Count();
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                var listData = this.dbcontext.Clon.Include(x => x.ListInstAccount).Include(x => x.ClonGroup).Include(x => x.PostGroup).ThenInclude(x => x.ListPost).Where(x => x.ClonGroupId == groupId).AsQueryable().ToList();

                if (lenght == -1)

                {
                    listData = listData.Skip(start).Take(data.recordsTotal).ToList();
                }
                else
                {
                    listData = listData.Skip(start).Take(lenght).ToList();
                }
                data.data = mapper.Map<List<GetClonDTO>>(listData.ToArray());
                data.recordsFiltered = listData.Count();
            }
            return data;
        }

        public async Task UpdateCloneAsync(DBClon data)
        {
            this.dbcontext.Clon.Update(data);
            await this.dbcontext.SaveChangesAsync();
        }
    }
}