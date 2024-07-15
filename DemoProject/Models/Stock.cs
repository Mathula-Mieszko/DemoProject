using System.ComponentModel.DataAnnotations.Schema;

namespace DemoProject.Models
{
    public class Stock
    {
       public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;

        public string CompanyName {  get; set; } = string.Empty;
        [Column(TypeName ="decimal(18,5)")]
        public decimal Price { get; set; }

        public List<Comment> Comment { get; set; } = new List<Comment>();

    }
}
