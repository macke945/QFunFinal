using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QFun.Data.DbTables;

namespace QFun.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Challenge> Challenge { get; set; }
        public DbSet<Contribution> Contribution { get; set; }
        public DbSet<Vote> Vote { get; set; }
        public DbSet<ApplicationUser> IdentityUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            ConfigureChallenge(modelBuilder);
            ConfigureContribution(modelBuilder);
            ConfigureVote(modelBuilder);
            //ConfigureIdentityUser(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        //private void ConfigureIdentityUser(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ApplicationUser>()
        //        .HasMany(a => a.Contributions)
        //        .WithOne(c => c.User)
        //        .HasForeignKey(c => c.UserId);

        //}

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
    }
}

