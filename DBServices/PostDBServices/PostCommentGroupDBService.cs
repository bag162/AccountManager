using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.Controllers.Post.Group.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices.PostDBServices
{
    public class PostCommentGroupDBService : IPostCommentGroupDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;

        public PostCommentGroupDBService(AMContext amcontext, IMapper mapper)
        {
            this.dbcontext = amcontext;
            this.mapper = mapper;
        }

        public async Task AddPostCommentGroupAsync(DBPostCommentGroup commentGroup)
        {
            await this.dbcontext.CommentGroup.AddAsync(commentGroup);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task DeleteCommentGroupAsync(List<DBPostCommentGroup> commentGroups)
        {
            this.dbcontext.CommentGroup.RemoveRange(commentGroups);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public DBPostCommentGroup GetCommentGroupById(int id)
        {
            var comments = this.dbcontext.CommentGroup.Include(x => x.ListComment).Where(x => x.Id == id).First();
            return comments;
        }

        public async Task<string[]> GetGroupNamesAsync()
        {
            return await this.dbcontext.CommentGroup.Select(x => x.Name).ToArrayAsync();
        }

        public JqueryDataTable<CommentGroupDTO> GetPostCommentGroups(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<CommentGroupDTO>();
            data.recordsTotal = this.dbcontext.PostGroup.Count();
            DBPostCommentGroup[] filteredData = Array.Empty<DBPostCommentGroup>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.CommentGroup.Include(x => x.ListPost).Include(x => x.ListComment).AsQueryable().Where(m => m.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.CommentGroup.Include(x => x.ListPost).Include(x => x.ListComment).AsQueryable().Where(m => m.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<CommentGroupDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<CommentGroupDTO>>(this.dbcontext.CommentGroup.Include(x => x.ListComment).Include(x => x.ListPost).AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<CommentGroupDTO>>(this.dbcontext.CommentGroup.Include(x => x.ListComment).Include(x => x.ListPost).AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }
    }
}
