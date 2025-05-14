using System.Security.Claims;
using FoodApp.Models;

namespace FoodApp.Repository
{
    public interface IData
    {
        Task<ApplicationUser> GetUser(ClaimsPrincipal claims
            );
    }
}
