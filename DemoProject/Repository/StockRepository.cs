using DemoProject.data;
using DemoProject.Dtos.Stock;
using DemoProject.Interfaces;
using DemoProject.Mappers;
using DemoProject.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public StockRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _dbContext.Stocks.AddAsync(stockModel);
            await _dbContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (stock == null)
            {
                return null;
            }
            _dbContext.Stocks.Remove(stock);
            await _dbContext.SaveChangesAsync();

            return stock;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            //return _dbContext.Stocks.FromSqlRaw("SELECT * FROM getStocks()").ToListAsync();
           return await _dbContext.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _dbContext.Stocks.FindAsync(id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (stock == null)
            {
                return null;
            }

            /*stockModel.Symbol = stockRequestDto.Symbol;
            stockModel.CompanyName = stockRequestDto.CompanyName;
            stockModel.Price = stockRequestDto.Price;

            _dbContext.SaveChanges();*/

            _dbContext.Entry(stock).CurrentValues.SetValues(stockDto);
            await _dbContext.SaveChangesAsync();

            return stock;
        }
    }
}
