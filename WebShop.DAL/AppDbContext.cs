using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebShop.Domain.Entity;

#nullable disable

namespace WebShop.DAL
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categorys { get; set; }
        public virtual DbSet<Country> Countrys { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<Manufacture> Manufactures { get; set; }
        public virtual DbSet<PriceChange> PriceChanges { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        public virtual DbSet<PurchaseItem> PurchaseItems { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseNpgsql("Host=localhost;Database=WebSHop;Username=Ban;Password=123");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp")
                .HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categorys");

                entity.HasKey(e => e.Name);


                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("countrys");

                entity.HasKey(e => e.Name);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("deliveries");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("address");

                entity.Property(e => e.UserId).HasColumnName("customer_id");

                entity.Property(e => e.DeliveryDate)
                    .HasColumnType("date")
                    .HasColumnName("delivery_date")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.ProductCount).HasColumnName("product_count");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_costomer_id");

                entity.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_product_id");
            });

            modelBuilder.Entity<Manufacture>(entity =>
            {
                entity.ToTable("manufactures");

                entity.HasKey(e => e.Name);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<PriceChange>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.DatePriceChange })
                    .HasName("price_change_pkey");

                entity.ToTable("price_change");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.DatePriceChange)
                    .HasColumnType("date")
                    .HasColumnName("date_price_change")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.NewPrice)
                    .HasPrecision(9, 3)
                    .HasColumnName("new_price");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PriceChanges)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_product_id");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("category_name");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("country_name");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(400)
                    .HasColumnName("description");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("image");

                entity.Property(e => e.ManufactureName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("manufacture_name");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("name");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("type");

                entity.Property(e => e.Weignt)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("weignt");

                entity.Property(e => e.InStock)
                    .IsRequired()
                    .HasColumnName("in_stock")
                    .HasDefaultValueSql("true");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_category_name");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CountryName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_country_name");

                entity.HasOne(d => d.Manufacture)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ManufactureName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_manufacture_name");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("purchases");

                entity.Property(e => e.PurchaseId)
                    .ValueGeneratedNever()
                    .HasColumnName("purchase_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.PurchaseDate)
                    .HasColumnType("date")
                    .HasColumnName("purchase_date");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Purchases)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_id");
            });

            modelBuilder.Entity<PurchaseItem>(entity =>
            {
                entity.HasKey(e => new { e.PurchaseId, e.ProductId })
                    .HasName("pk_purchase_items");

                entity.ToTable("purchase_items");

                entity.Property(e => e.PurchaseId).HasColumnName("purchase_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.ProductCount).HasColumnName("product_count");

                entity.Property(e => e.ProductPrice)
                    .HasPrecision(9, 3)
                    .HasColumnName("product_price");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PurchaseItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_product_id");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.PurchaseItems)
                    .HasForeignKey(d => d.PurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_purchase_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("last_name");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(80)
                    .HasColumnName("role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
