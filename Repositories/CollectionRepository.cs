using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Recipedia.Data;
using Recipedia.Exceptions;
using Recipedia.Models;
using Recipedia.ResultObjects;
using System.Security.Claims;

namespace Recipedia.Repositories
{
    public interface ICollectionRepository
    {
        public Task<Collection> GetCollectionAsync(int collectionId, ClaimsPrincipal userClaims);
        public Task<List<Collection>> GetAllCollectionAsync(ClaimsPrincipal userClaims);
        public Task<OperationResult> CreateOrUpdateCollectionAsync(Collection collection, ClaimsPrincipal userClaims);
        public Task<OperationResult> DeteteCollectionAsync(int collectionId, ClaimsPrincipal userClaims);
    }

    public class CollectionRepository : ICollectionRepository
    {
        private readonly ApplicationContext _context;

        public CollectionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Collection> GetCollectionAsync(int collectionId, ClaimsPrincipal userClaims)
        {
            User user = await GetUserAndCollectionsAsync(userClaims);

            var collection = user.Collections.Where(c => c.Equals(collectionId)).SingleOrDefault();

            return collection;
        }

        public async Task<List<Collection>> GetAllCollectionAsync(ClaimsPrincipal userClaims)
        {
            User user = await GetUserAndCollectionsAsync(userClaims);

            var collections = user.Collections.ToList();

            if (collections.IsNullOrEmpty())
                return null;

            return collections;
        }

        public async Task<OperationResult> CreateOrUpdateCollectionAsync(Collection collection, ClaimsPrincipal userClaims)
        {
            User user = await GetUserAndCollectionsAsync(userClaims);

            var existingCollection = user.Collections.Single(c => c.CollectionId == collection.CollectionId);
            // If collection exists update it
            if (existingCollection != null)
            {
                existingCollection.Title = collection.Title;
                existingCollection.ImageUrl = collection.ImageUrl;
                existingCollection.TotalRecipes = collection.TotalRecipes;
              
                await _context.SaveChangesAsync();

                return OperationResult.Successful("Successfully updated collection.");
            }
            // If collection doesn't exist create a new one
            else if (existingCollection == null)
            {
                user.Collections.Add(collection);
                await _context.SaveChangesAsync();

                return OperationResult.Successful("Successfully created collection.");
            }

            // This will never run ?!
            return OperationResult.Failed("Failed to update or create collection");
        }

        public async Task<OperationResult> DeteteCollectionAsync(int collectionId, ClaimsPrincipal userClaims)
        {
            User user = await GetUserAndCollectionsAsync(userClaims);

            var collection = user.Collections.SingleOrDefault(c => c.CollectionId == collectionId);
            if (collection == null)
                return OperationResult.Failed("Collection was not found.", 404);

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();

            return OperationResult.Successful("Successfully deleted collection.");
        }



        // Help methods
        private async Task<User> GetUserAndCollectionsAsync(ClaimsPrincipal userClaims)
        {
            // Get the email from the JWT claims
            string? email = userClaims.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
                throw new UserNotFoundException("No email found in token claims.");

            // Get user along with its saved job ads
            User? user = await _context.Users
                .Include(u => u.Collections)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new UserNotFoundException("No matching user found with the provided email.");

            return user;
        }

    }
}
