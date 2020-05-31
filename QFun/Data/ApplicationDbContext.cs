using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QFun.Data.DbTables;

namespace QFun.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Challenge> Challenge { get; set; }
        public DbSet<Contribution> Contribution { get; set; }
        public DbSet<Vote> Vote { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            ConfigureChallenge(modelBuilder);
            ConfigureContribution(modelBuilder);
            ConfigureVote(modelBuilder);
            ConfigureApplicationUser(modelBuilder);
            SeedDataBase(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureApplicationUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.Contributions)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
        }

        private void ConfigureChallenge(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Challenge>()
                 .HasMany(c => c.Contributions)
                 .WithOne(p => p.Challenge)
                 .HasForeignKey(p => p.ChallengeId);
        }

        private void ConfigureContribution(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contribution>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);
            modelBuilder.Entity<Contribution>()
                .HasOne(p => p.Challenge)
                .WithMany(c => c.Contributions)
                .HasForeignKey(p => p.ChallengeId);
            modelBuilder.Entity<Contribution>()
                .HasMany(p => p.Votes)
                .WithOne(v => v.Contribution)
                .HasForeignKey(v => v.ContributionId);
        }

        private void ConfigureVote(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vote>()
               .HasKey(l => new { l.UserId, l.ContributionId });
        }


        private static void SeedDataBase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Challenge>().HasData(
                new Challenge { Id = 1, Title = "William Shakespeare", Description = "yes" },
                new Challenge { Id = 2, Title = "Will", Description = "2222"},
                new Challenge { Id = 3, Title = "Robert C. Martin", Description = "123" }
            );

            modelBuilder.Entity<Contribution>().HasData(
                new Contribution { Id = 1, ChallengeId = 1, Description = "i did this", Path = "imagehere", TimeOfUpload = Convert.ToDateTime("2020-05-03")},
                new Contribution { Id = 2, ChallengeId = 1, Description = "my test 1", Path = "test1", TimeOfUpload = Convert.ToDateTime("2020-05-04")},
                new Contribution { Id = 3, ChallengeId = 2, Description = "my test 2", Path = "test2", TimeOfUpload = Convert.ToDateTime("2020-05-14")},
                new Contribution { Id = 4 , ChallengeId = 2, Description = "my test 3", Path = "test3", TimeOfUpload = Convert.ToDateTime("2020-05-24")},
                new Contribution { Id = 5, ChallengeId = 3, Description = "my test 4", Path = "test4", TimeOfUpload = Convert.ToDateTime("2020-05-25")}
                );
        }
    }
}

