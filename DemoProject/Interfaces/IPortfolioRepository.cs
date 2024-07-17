using DemoProject.Models;

namespace DemoProject.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPorfolio(AppUser user);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio> DeletePortfolioAsync(AppUser user, string symbol);
    }

}
