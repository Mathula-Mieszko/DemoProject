using System.ComponentModel.DataAnnotations;

namespace DemoProject.Dtos.Comment
{
    public class CreateCommentRequestDto
    {
        [Required]
        [MinLength(5,ErrorMessage ="Title must be 5 character")]
        [MaxLength(10,ErrorMessage ="Title can't be over 10 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
