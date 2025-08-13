using Microsoft.EntityFrameworkCore;
using VWProcurement.Core.Models;

namespace VWProcurement.Data
{
    public class VWProcurementDbContext : DbContext
    {
        public VWProcurementDbContext(DbContextOptions<VWProcurementDbContext> options) : base(options) { }

        // Start with just basic entities
        public DbSet<User> Users { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<TenderCategory> TenderCategories { get; set; }
        public DbSet<Tender> Tenders { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User configurations
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
                entity.Property(e => e.PasswordHash).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Role).HasDefaultValue(UserRole.PublicViewer);
                entity.Property(e => e.Status).HasDefaultValue(UserStatus.Pending);
                entity.Property(e => e.EmailVerified).HasDefaultValue(false);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");

                // Specify decimal precision
                entity.Property(e => e.AnnualTurnover).HasColumnType("decimal(18,2)");

                // Ignore ambiguous navigations not explicitly mapped
                entity.Ignore(e => e.CreatedTenders);
                entity.Ignore(e => e.Bids);
                entity.Ignore(e => e.Questions);
                entity.Ignore(e => e.AuditLogs);
            });

            // Supplier configurations
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                      .WithOne()
                      .HasForeignKey<Supplier>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.CompanyName).HasMaxLength(255).IsRequired();
                entity.Property(e => e.BusinessRegistrationNumber).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.BusinessRegistrationNumber).IsUnique();
                entity.Property(e => e.IsVerified).HasDefaultValue(false);
                entity.Property(e => e.AnnualTurnover).HasColumnType("decimal(18,2)");
            });

            // Buyer configurations
            modelBuilder.Entity<Buyer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                      .WithOne()
                      .HasForeignKey<Buyer>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.OrganizationName).HasMaxLength(255).IsRequired();
                entity.Property(e => e.IsVerified).HasDefaultValue(false);
            });

            // Manager configurations
            modelBuilder.Entity<Manager>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                      .WithOne()
                      .HasForeignKey<Manager>(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            });

            // TenderCategory configurations
            modelBuilder.Entity<TenderCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            // Tender configurations
            modelBuilder.Entity<Tender>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TenderNumber).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.TenderNumber).IsUnique();
                entity.Property(e => e.Title).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Status).HasDefaultValue(TenderStatus.Draft);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.MinimumScore).HasColumnType("decimal(5,2)");
                
                // Foreign keys
                entity.HasOne(e => e.Buyer)
                .WithMany()
                      .HasForeignKey(e => e.BuyerId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Category)
                      .WithMany()
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Ignore optional navigations to avoid ambiguity for now
                entity.Ignore(e => e.ApprovedByManager);
                entity.Ignore(e => e.AwardedToSupplier);
            });

            // Bid configurations
            modelBuilder.Entity<Bid>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BidNumber).HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.BidNumber).IsUnique();
                entity.Property(e => e.Status).HasDefaultValue(BidStatus.Submitted);
                entity.Property(e => e.ValidityPeriod).HasDefaultValue(90);
                entity.Property(e => e.SubmissionDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
                
                // Foreign keys
            entity.HasOne(e => e.Tender)
                .WithMany(t => t.Bids)
                      .HasForeignKey(e => e.TenderId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
            entity.HasOne(e => e.Supplier)
                .WithMany(s => s.Bids)
                      .HasForeignKey(e => e.SupplierId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                // Unique constraint - one bid per supplier per tender
                entity.HasIndex(e => new { e.TenderId, e.SupplierId }).IsUnique();
            });

        // TenderQuestion configurations
        modelBuilder.Entity<TenderQuestion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Tender)
                .WithMany(t => t.Questions)
                .HasForeignKey(e => e.TenderId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.User)
                .WithMany(u => u.Questions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.AnsweredByUser)
                .WithMany()
                .HasForeignKey(e => e.AnsweredBy)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // TenderDocument configurations
        modelBuilder.Entity<TenderDocument>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Tender)
                .WithMany(t => t.Documents)
                .HasForeignKey(e => e.TenderId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.UploadedByUser)
                .WithMany()
                .HasForeignKey(e => e.UploadedBy)
                .OnDelete(DeleteBehavior.Restrict);
        });

            base.OnModelCreating(modelBuilder);
        }
    }
}
