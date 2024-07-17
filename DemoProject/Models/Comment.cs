using System.ComponentModel.DataAnnotations.Schema;

namespace DemoProject.Models
{
    [Table("Comments")]

    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty ;

        public int? StockId { get; set; }

        public Stock? Stock { get; set; }

        public string AppUserId {  get; set; }
        public AppUser AppUser { get; set; }
    }
}
