using Cs.Unicam.TrashHunter.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs.Unicam.TrashHunter.Models.DB
{
    public class TrashHunterContext : DbContext
    {
        public TrashHunterContext() 
        { 
        }

        public TrashHunterContext(DbContextOptions<TrashHunterContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(u =>
            {
                u.ToTable("Users");
                u.HasKey(u => u.Email);
                u.Property(u => u.Role).HasConversion(new EnumToStringConverter<Role>());
                u.HasMany(u => u.Post).WithOne(p => p.User).HasForeignKey(p => p.UserCode).OnDelete(DeleteBehavior.NoAction);
                u.HasMany(u => u.PostCompleted).WithOne(p => p.CompletedByUser).HasForeignKey(p => p.CompletedBy).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
            });
            modelBuilder.Entity<Post>(p =>
            {
                p.ToTable("Posts");
                p.HasKey(p => p.PostId);
            });
            modelBuilder.Entity<Attachment>(a =>
            {
                a.ToTable("Attachments");
                a.HasKey(a => a.FileName);
                a.HasOne(a => a.Post).WithMany(p => p.Attachments).HasForeignKey(a => a.PostId).IsRequired(false);
                a.HasOne(a => a.Company).WithOne(c => c.AdviceImage).HasForeignKey<Attachment>(a => a.CompanyCode).IsRequired(false);
                a.HasOne(a => a.CompanyLogo).WithOne(c => c.Logo).HasForeignKey<Attachment>(a => a.CompanyLogoCode).IsRequired(false);
                a.ToTable(a => a.HasCheckConstraint("CK_AtLeastOne", "PostId IS NOT NULL OR CompanyCode IS NOT NULL or CompanyLogoCode IS NOT NULL"));
            });
            modelBuilder.Entity<Company>(c =>
            {
                c.ToTable("Companies");
                c.HasKey(c => c.CompanyCode);
                c.HasKey(c => c.CompanyCode);
                c.HasMany(c => c.Posts).WithOne(p => p.Company).HasForeignKey(p => p.CompanyCode).IsRequired(false);
            });
            modelBuilder.Entity<Cupon>(c =>
            {
                c.ToTable("Cupons");
                c.HasKey(c => c.CuponId);
                c.HasOne(c => c.Company).WithMany(c => c.Cupons).HasForeignKey(c => c.CompanyCode);
                c.HasOne(c => c.User).WithMany(u => u.Cupons).HasForeignKey(c => c.UserCode);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<Attachment> Attachments { get; internal set; }
        public DbSet<Post> Posts { get; internal set; }
        public DbSet<User> Users { get; internal set; }
        public DbSet<Company> Companies { get; internal set; }
        public DbSet<Cupon> Cupons { get; internal set; }



    }
}
