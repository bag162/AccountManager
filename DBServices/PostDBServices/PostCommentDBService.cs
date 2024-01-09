using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BASAccountManager.DBServices.PostDBServices
{
    public class PostCommentDBService : IPostCommentDBService
    {
        private AMContext dbcontext;
        private ILogger<PostCommentDBService> logger;
        private IMapper mapper;

        public PostCommentDBService(AMContext amcontext, ILogger<PostCommentDBService> logger, IMapper mapper)
        {
            this.dbcontext = amcontext; ;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task AddCommentAsync(List<CRUDCommentDTO> comments)
        {
            var group = this.dbcontext.CommentGroup.Where(x => x.Name == comments.First().CommentGroupName).First().Id;
            var newComments = mapper.Map<List<DBComment>>(comments);

            foreach (var comment in newComments)
            {
                comment.CommentGroupId = group;
            }

            await this.dbcontext.Comment.AddRangeAsync(newComments);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task AddCommentAsync(List<DBPostComment> comments)
        {
            await this.dbcontext.PostComment.AddRangeAsync(comments);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public async Task DeleteCommentAsync(List<CRUDCommentDTO> comments)
        {
            var deletedComments = mapper.Map<List<DBComment>>(comments);
            this.dbcontext.Comment.RemoveRange(deletedComments);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public JqueryDataTable<CommentDTO> GetComments(int start, int lenght, string searchdata)
        {
            var data = new JqueryDataTable<CommentDTO>();
            data.recordsTotal = this.dbcontext.Comment.Count();
            DBComment[] filteredData = Array.Empty<DBComment>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.Comment.Include(x => x.CommentGroup).AsQueryable()
                                                .Where(m => m.Message.Contains(searchdata)
                                                || m.CommentGroup.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.Comment.Include(x => x.CommentGroup).AsQueryable().Where(m => m.Message.Contains(searchdata)
                                                || m.CommentGroup.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<CommentDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<CommentDTO>>(this.dbcontext.Comment.Include(x => x.CommentGroup).AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<CommentDTO>>(this.dbcontext.Comment.Include(x => x.CommentGroup).AsQueryable().Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public JqueryDataTable<CommentDTO> GetCommentsByGroup(int start, int lenght, string searchdata, int groupId)
        {
            var data = new JqueryDataTable<CommentDTO>();
            data.recordsTotal = this.dbcontext.Comment.Count();
            DBComment[] filteredData = Array.Empty<DBComment>();

            if (!string.IsNullOrEmpty(searchdata))
            {
                if (lenght == -1)
                {
                    filteredData = this.dbcontext.Comment.Include(x => x.CommentGroup).AsQueryable()
                                                .Where(m => m.Message.Contains(searchdata)
                                                || m.CommentGroup.Name.Contains(searchdata)
                                                || m.CommentGroup.Id.Equals(groupId)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.Comment.Include(x => x.CommentGroup).AsQueryable().Where(m => m.Message.Contains(searchdata)
                                                || m.CommentGroup.Name.Contains(searchdata)
                                                || m.CommentGroup.Id.Equals(groupId)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(lenght).ToArray();
                }


                data.recordsFiltered = filteredData.Count();
                data.data = mapper.Map<List<CommentDTO>>(filteredData);
            }
            else
            {
                data.recordsFiltered = data.recordsTotal;
                if (lenght == -1)
                {
                    data.data = mapper.Map<List<CommentDTO>>(this.dbcontext.Comment.Include(x => x.CommentGroup).AsQueryable().Where(x => x.CommentGroup.Id.Equals(groupId)).Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<CommentDTO>>(this.dbcontext.Comment.Include(x => x.CommentGroup).AsQueryable().Where(x => x.CommentGroup.Id.Equals(groupId)).Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }
    }
}
