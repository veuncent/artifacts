using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Artifacts.Models;
using Artifacts.Models.Entities;

namespace Artifacts.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        //public override async Task<int> SaveChangesAsync()
        //{
        //    AddTimestamps();
        //    return await base.SaveChangesAsync();
        //}

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            var now = DateTime.Now;

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).Created = now;
                }

                ((BaseEntity)entity.Entity).Edited = now;
            }
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<BlogPostImage> BlogPostImages { get; set; }
        public DbSet<BlogPostTag> Tags { get; set; }
    }
}
