using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BenFatto.CLF.Model
{
    public partial class ClfContext : DbContext
    {
        public readonly string cnStr;
        public ClfContext() : base()
        {
            cnStr = DbConfiguration.Current.ConnectionString;
        }

        public virtual DbSet<Import> Imports { get; set; }
        public virtual DbSet<LogRow> LogRows { get; set; }
        public virtual DbSet<LogRowMismatch> LogRowMismatches { get; set; }

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

            modelBuilder.Entity<Import>(entity =>
            {
                entity.ToTable("Import");

                entity.Property(e => e.ErrorCount).HasDefaultValueSql("0");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.MismatchRowsFileName)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.RowCount).HasDefaultValueSql("0");

                entity.Property(e => e.SuccessCount).HasDefaultValueSql("0");

                entity.Property(e => e.When).HasColumnType("date");
            });

            modelBuilder.Entity<LogRow>(entity =>
            {
                entity.ToTable("LogRow");

                entity.Property(e => e.Date).HasColumnType("timestamp");

                entity.Property(e => e.IpAddress)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.OriginalLine)
                    .IsRequired()
                    .HasMaxLength(8192);

                entity.Property(e => e.Protocol)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Referer).HasMaxLength(1024);

                entity.Property(e => e.Resource)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.RfcId)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserAgent).HasMaxLength(4096);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Import)
                    .WithMany(p => p.LogRows)
                    .HasForeignKey(d => d.ImportId)
                    .HasConstraintName("FK_Import_LogRow");
            });

            modelBuilder.Entity<LogRowMismatch>(entity =>
            {
                entity.ToTable("LogRowMismatch");

                entity.Property(e => e.Row)
                    .IsRequired()
                    .HasMaxLength(8096);

                entity.Property(e => e.ThrownException)
                    .IsRequired()
                    .HasMaxLength(10240);

                entity.HasOne(d => d.Import)
                    .WithMany(p => p.LogRowMismatches)
                    .HasForeignKey(d => d.ImportId)
                    .HasConstraintName("FK_Import_LogRowMismatch");

                entity.Property(e => e.Corrected).HasColumnType("boolean").HasDefaultValue(false);
                entity.Property(e => e.CorrectedAt).HasColumnType("date").HasDefaultValue(DateTime.MaxValue);

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
