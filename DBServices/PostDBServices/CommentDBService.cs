using AutoMapper;
using BASAccountManager.Controllers.DTO;
using BASAccountManager.Controllers.Post.Comment.DTO;
using BASAccountManager.DB;
using BASAccountManager.DB.Models.Post;
using BASAccountManager.DBServices.PostDBServices.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BASAccountManager.DBServices.PostDBServices
{
    public class CommentDBService : ICommentDBService
    {
        private AMContext dbcontext;
        private IMapper mapper;

        public CommentDBService(AMContext amcontext, IMapper mapper)
        {
            this.dbcontext = amcontext;
            this.mapper = mapper;
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
                    filteredData = this.dbcontext.Comment.Include(x => x.PostCommentGroup).AsQueryable()
                                                .Where(m => m.Message.Contains(searchdata)
                                                || m.PostCommentGroup.Name.Contains(searchdata)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.Comment.Include(x => x.PostCommentGroup).AsQueryable().Where(m => m.Message.Contains(searchdata)
                                                || m.PostCommentGroup.Name.Contains(searchdata)
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
                    data.data = mapper.Map<List<CommentDTO>>(this.dbcontext.Comment.Include(x => x.PostCommentGroup).AsQueryable().Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<CommentDTO>>(this.dbcontext.Comment.Include(x => x.PostCommentGroup).AsQueryable().Skip(start).Take(lenght).ToArray());
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
                    filteredData = this.dbcontext.Comment.Include(x => x.PostCommentGroup).AsQueryable()
                                                .Where(m => m.Message.Contains(searchdata)
                                                || m.PostCommentGroup.Name.Contains(searchdata)
                                                || m.PostCommentGroup.Id.Equals(groupId)
                                                || m.Id.ToString().Equals(searchdata)).Skip(start).Take(data.recordsTotal).ToArray();
                }
                else
                {

                    filteredData = this.dbcontext.Comment.Include(x => x.PostCommentGroup).AsQueryable().Where(m => m.Message.Contains(searchdata)
                                                || m.PostCommentGroup.Name.Contains(searchdata)
                                                || m.PostCommentGroup.Id.Equals(groupId)
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
                    data.data = mapper.Map<List<CommentDTO>>(this.dbcontext.Comment.Include(x => x.PostCommentGroup).AsQueryable().Where(x => x.PostCommentGroup.Id.Equals(groupId)).Skip(start).Take(data.recordsTotal).ToArray());
                }
                else
                {
                    data.data = mapper.Map<List<CommentDTO>>(this.dbcontext.Comment.Include(x => x.PostCommentGroup).AsQueryable().Where(x => x.PostCommentGroup.Id.Equals(groupId)).Skip(start).Take(lenght).ToArray());
                }
            }
            return data;
        }

        public async Task AddCommentAsync(List<CRUDCommentDTO> comments)
        {
            var newComments = new List<DBComment>();
            foreach (var comment in comments)
            {
                var newComment = mapper.Map<DBComment>(comment);
                newComment.PostCommentGroupId = this.dbcontext.CommentGroup.AsQueryable().Where(x => x.Name == comment.CommentGroupName).First().Id;
                newComment.CreatedDate = DateTime.Now;
                newComments.Add(newComment);
            }

            await this.dbcontext.Comment.AddRangeAsync(newComments);
            await this.dbcontext.SaveChangesAsync();
            return;
        }
        public async Task RemoveCommentsAsync(List<CRUDCommentDTO> comments)
        {
            var deletedComments = mapper.Map<List<DBComment>>(comments);
            this.dbcontext.Comment.RemoveRange(deletedComments);
            await this.dbcontext.SaveChangesAsync();
            return;
        }

        public List<DBComment> GetCommentsByGroup(string group)
        {
            return this.dbcontext.Comment.Include(x => x.PostCommentGroup).AsQueryable().Where(x => x.PostCommentGroup.Name == group).ToList();
        }
    }
}
