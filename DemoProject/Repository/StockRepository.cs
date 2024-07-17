using DemoProject.data;
using DemoProject.Dtos.Stock;
using DemoProject.Helpers;
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

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            //return _dbContext.Stocks.FromSqlRaw("SELECT * FROM getStocks()").ToListAsync();
            var stocks =  _dbContext.Stocks.Include(c=>c.Comment).ThenInclude(u => u.AppUser).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s=>s.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s=>s.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol",StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending? stocks.OrderByDescending(s=>s.Symbol):stocks.OrderBy(s=>s.Symbol);
                }
            }
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _dbContext.Stocks.Include(c => c.Comment).ThenInclude(u=>u.AppUser).FirstOrDefaultAsync(s=>s.Id== id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

        public async Task<bool> StockExist(int id)
        {
            return await _dbContext.Stocks.AnyAsync(s => s.Id == id);
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
