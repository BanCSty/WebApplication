using Auth.Entity;
using Microsoft.EntityFrameworkCore;


#nullable disable

namespace Auth.DAL
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
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens{get;set;}

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

            modelBuilder.Entity<RefreshToken>(entity =>
            {

                entity.ToTable("refresh_token");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id");

                entity.Property(e => e.TokenRefresh)
                    .HasColumnType("text")
                    .HasColumnName("refresh_token");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("date")
                    .HasColumnName("expiration_date");

                entity.HasOne(d => d.IdUserNavigation)
                      .WithMany()
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("refresh_token_user_id_fkey");
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
                    .HasMaxLength(40)
                    .HasColumnName("role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
