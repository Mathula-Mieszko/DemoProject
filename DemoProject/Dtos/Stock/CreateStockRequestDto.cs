using System.ComponentModel.DataAnnotations;

namespace DemoProject.Dtos.Stock
{
    public class CreateStockRequestDto
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        [Range(1,100)]
        public decimal Price { get; set; }
    }
}
