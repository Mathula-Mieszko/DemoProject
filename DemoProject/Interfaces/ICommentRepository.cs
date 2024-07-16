using DemoProject.Dtos.Comment;
using DemoProject.Models;

namespace DemoProject.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);

        Task<Comment> CreateAsync(Comment commentModel);

        //Task<Comment?> UpdateAsync(int id, Comment commentModel);

        Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto updateCommentRequestDto);

        Task<Comment?> DeleteAsync(int id);
    }
}
