using DemoProject.data;
using DemoProject.Dtos.Comment;
using DemoProject.Dtos.Stock;
using DemoProject.Interfaces;
using DemoProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace DemoProject.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Comment>> GetAllAsync()
        {
            return await _dbContext.Comments.Include(c=>c.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _dbContext.Comments.Include(c => c.AppUser).FirstOrDefaultAsync(c=>c.Id == id);
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _dbContext.Comments.AddAsync(commentModel);
            await _dbContext.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto updateCommentRequestDto)
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment == null)
            {
                return null;
            }
            _dbContext.Entry(comment).CurrentValues.SetValues(updateCommentRequestDto);
            await _dbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
           var comment = await _dbContext.Comments.FirstOrDefaultAsync(c=>c.Id == id);
            if (comment == null) {
                return null;
            }

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();

            return comment;
        }

        /* public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
         {
             var comment = await _dbContext.Comments.FindAsync(id);
             if (comment == null)
             {
                 return null;
             }
             comment.Title = commentModel.Title;
             comment.Content = commentModel.Content;
            // _dbContext.Entry(comment).CurrentValues.SetValues(commentModel);
             await _dbContext.SaveChangesAsync();
             return comment;
         }*/


    }
}
