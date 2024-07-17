using DemoProject.Models;

namespace DemoProject.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
