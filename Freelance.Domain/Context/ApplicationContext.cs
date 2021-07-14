using Freelance.Domain.Entities;
using Freelance.Shared.Enumerations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Freelance.Domain.Context
{
    public class ApplicationContext : IdentityDbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public void OnModelCreating(ModelBuilder builder)
        {
            //UserCategories
            builder.Entity<UserCategory>()
                .HasKey(b => new { b.CategoryId, b.UserId });
            builder.Entity<UserCategory>()
                .HasOne(b => b.Category)
                .WithMany(b => b.UserCategories)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserCategory>()
                .HasOne(b => b.UserProfile)
                .WithMany(b => b.UserCategories)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Category>()
                .HasKey(b => b.Id);
            builder.Entity<UserProfile>()
                .HasKey(b => b.Id);

            //JobCategory many-to-many
            builder.Entity<JobCategory>()
                .HasKey(b => b.Id);
            builder.Entity<JobCategory>()
                .HasOne(b => b.Category)
                .WithMany(b => b.JobCategories)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<JobCategory>()
                .HasOne(b => b.JobOffer)
                .WithMany(b => b.JobCategories)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserProfile>()
                .HasOne(s => s.User) 
                .WithOne(s => s.UserProfile);

            builder.Entity<EmployerProfile>()
                .HasOne(s => s.User)
                .WithOne(s => s.EmployerProfile);

            //JobCategory many-to-many
            builder.Entity<UserCategory>()
               .HasKey(b => new { b.UserId, b.CategoryId });
            builder.Entity<UserCategory>()
                .HasOne(b => b.UserProfile)
                .WithMany(b => b.UserCategories)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserCategory>()
                .HasOne(b => b.Category)
                .WithMany(b => b.UserCategories)
                .OnDelete(DeleteBehavior.Restrict);

            //Bid many-to-many
            builder.Entity<Bid>()
               .HasKey(b => b.Id);
            builder.Entity<Bid>()
                .HasOne(b => b.Job)
                .WithMany(b => b.Bids)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Bid>()
                .HasOne(b => b.UserProfile)
                .WithMany(b => b.Bids)
                .OnDelete(DeleteBehavior.Restrict);

            //Notification
            builder.Entity<Notification>()
               .HasKey(b => b.Id);
            builder.Entity<Notification>()
                .HasOne(b => b.Job)
                .WithMany(b => b.Notifications)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Notification>()
                .HasOne(b => b.UserProfile)
                .WithMany(b => b.Notifications)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<JobOffer>()
                .HasOne(b => b.Employer)
                .WithMany(b => b.JobOffers)
                .OnDelete(DeleteBehavior.Restrict);

        }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCategory> UserCategories { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<EmployerProfile> EmployerProfiles { get; set; }

        public override int SaveChanges()
        {
            var commonObjectSet = ChangeTracker.Entries<CommonFields>()
                                               .Where(c => c.State == EntityState.Added || c.State == EntityState.Modified)
                                               .ToList();

            if (commonObjectSet != null)
            {
                foreach (var entry in commonObjectSet)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.Status = EntityStatus.Active;
                            entry.Entity.CreateDate = DateTime.Now;
                            entry.Entity.LastModifyDate = DateTime.Now;
                            break;
                        case EntityState.Modified:
                            entry.Entity.LastModifyDate = DateTime.Now;
                            break;
                    }
                }
            }
            int result = base.SaveChanges();
            return result;
        }

        public void Commit()
        {
            try
            {
                this.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                throw ex;
            }
        }
    }
}
