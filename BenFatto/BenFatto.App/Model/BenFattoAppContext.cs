using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BenFatto.App.DTO;

#nullable disable

namespace BenFatto.App.Model
{
    public partial class BenFattoAppContext : DbContext
    {
        public readonly string cnStr;
        public BenFattoAppContext() : base()
        {
            cnStr = DbConfiguration.Current.ConnectionString;
        }

        public virtual DbSet<Functionality> Functionalities { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserFunctionality> UserFunctionalities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(cnStr);
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Portuguese_Brazil.1252");

            modelBuilder.Entity<Functionality>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Route)
                    .IsRequired()
                    .HasMaxLength(1024);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("Email");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<UserFunctionality>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.FunctionalityId });

                entity.HasOne(d => d.Functionality)
                    .WithMany(p => p.UserFunctionalities)
                    .HasForeignKey(d => d.FunctionalityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Functionalities_UserFunctionalities");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFunctionalities)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Users_UserFunctionalities");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
