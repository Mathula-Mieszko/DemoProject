using DemoProject.Dtos.Stock;
using DemoProject.Models;

namespace DemoProject.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Price = stockModel.Price,
            };
        }

        public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockRequestDto) {
            return new Stock{ 
                Symbol = stockRequestDto.Symbol,
                CompanyName = stockRequestDto.CompanyName,
                Price = stockRequestDto.Price
            };
        }



    }
}
