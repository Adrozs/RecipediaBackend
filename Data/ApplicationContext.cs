using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Recipedia.Models;
using System.Collections.Generic;

namespace Recipedia.Data
{
    public class ApplicationContext: IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Shoppinglist> Shoppinglists { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var instructionsComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2), // Comparison logic
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Hash code generation
                c => c.ToList() // Cloning logic
            );


            // Recipe
            modelBuilder.Entity<Recipe>()
                .OwnsOne(r => r.Allergens);

            modelBuilder.Entity<Recipe>()
                .OwnsOne(r => r.Ingredients);    
            
            modelBuilder.Entity<Recipe>()
                .OwnsOne(r => r.Nutrition);

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Instructions)
                .HasConversion(
                    v => string.Join(';', v),  // Serialize list to a single string
                    v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList() // Deserialize string to a list
                )
                .Metadata.SetValueComparer(instructionsComparer);



            // Shoppinglist
            modelBuilder.Entity<Shoppinglist>()
                .OwnsOne(r => r.Items);


            // If you have other configurations
            base.OnModelCreating(modelBuilder);
        }

    }
}
