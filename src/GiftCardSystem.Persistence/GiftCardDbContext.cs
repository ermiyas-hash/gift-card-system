using Microsoft.EntityFrameworkCore;
using GiftCardSystem.Domain.Entities;

namespace GiftCardSystem.Persistence
{
    public class GiftCardDbContext : DbContext
    {
        public GiftCardDbContext(DbContextOptions<GiftCardDbContext> options) : base(options)
        {
        }

        public DbSet<GiftCard> GiftCards { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Redemption> Redemptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // GiftCard configuration
            modelBuilder.Entity<GiftCard>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.Property(e => e.InitialBalance).HasPrecision(18, 2);
                entity.Property(e => e.CurrentBalance).HasPrecision(18, 2);
                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.GiftCards)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(e => e.Transactions)
                    .WithOne(t => t.GiftCard)
                    .HasForeignKey(t => t.GiftCardId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            });

            // Transaction configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Reference).HasMaxLength(100);
            });

            // Redemption configuration
            modelBuilder.Entity<Redemption>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasOne(e => e.GiftCard)
                    .WithMany()
                    .HasForeignKey(e => e.GiftCardId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
