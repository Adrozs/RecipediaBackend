using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Recipedia.Data;
using Recipedia.Exceptions;
using Recipedia.Models;
using System.Security.Claims;

namespace Recipedia.Repositories
{
    public interface IShoppinglistRepository
    {
        public Task<Shoppinglist> GetShoppinglistAsync(int shoppinglistId, ClaimsPrincipal userClaims);
        public Task<List<Shoppinglist>> GetAllShoppinglistsAsync(ClaimsPrincipal userClaims);

        // SaveOrUpdate

        // Delete
    }

    public class ShoppinglistRepository : IShoppinglistRepository
    {
        private readonly ApplicationContext _context;

        public ShoppinglistRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<Shoppinglist> GetShoppinglistAsync(int shoppinglistId, ClaimsPrincipal userClaims)
        {
            User user = await GetUserAndShoppinglistsAsync(userClaims);

            var shoppinglist = user.Shoppinglists.Where(s => s.Equals(shoppinglistId)).SingleOrDefault();

            return shoppinglist;
        }

        public async Task<List<Shoppinglist>> GetAllShoppinglistsAsync(ClaimsPrincipal userClaims)
        {
            User user = await GetUserAndShoppinglistsAsync(userClaims);

            var shoppinglists = user.Shoppinglists.ToList();

            if (shoppinglists.IsNullOrEmpty())
                return null;

            return shoppinglists;
        }

        // Help methods
        private async Task<User> GetUserAndShoppinglistsAsync(ClaimsPrincipal userClaims)
        {
            // Get the email from the JWT claims
            string? email = userClaims.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                throw new UserNotFoundException("No email found in token claims.");

            // Get user along with its saved job ads
            User? user = await _context.Users
                .Include(u => u.Shoppinglists)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new UserNotFoundException("No matching user found with the provided email.");

            return user;
        }
    }
}
