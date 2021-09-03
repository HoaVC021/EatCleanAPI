using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace EatCleanAPI.Models
{
    public partial class VegafoodBotContext : DbContext
    {
        public VegafoodBotContext()
        {
        }

        public VegafoodBotContext(DbContextOptions<VegafoodBotContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CustomerSentiment> CustomerSentiments { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenusDetail> MenusDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=Vegafood");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerSentiment>(entity =>
            {
                entity.ToTable("CustomerSentiment");

                entity.Property(e => e.CustomerName).HasMaxLength(30);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FoodComment).HasMaxLength(30);

                entity.Property(e => e.FoodPredict)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.NameByUser).HasMaxLength(30);

                entity.Property(e => e.Phone).HasMaxLength(10);

                entity.Property(e => e.ServiceComment).HasMaxLength(30);

                entity.Property(e => e.ServicePredict)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.VegaComment).HasMaxLength(30);

                entity.Property(e => e.VegaPredict)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.MenuId).HasColumnName("menuId");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("ntext");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasColumnType("ntext");

                entity.Property(e => e.IsDisabled).HasColumnName("isDisabled");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<MenusDetail>(entity =>
            {
                entity.HasKey(e => new { e.MenuId, e.ProductId })
                    .HasName("menusdetail_pk");

                entity.ToTable("MenusDetail");

                entity.Property(e => e.MenuId).HasColumnName("menuId");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MenusDetails)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenusDetail_Menus");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.MenusDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MenusDetail_Products");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasColumnType("ntext");

                entity.Property(e => e.DeliveredAt)
                    .HasColumnName("deliveredAt")
                    .HasColumnType("ntext");

                entity.Property(e => e.EmailAddress)
                    .HasColumnName("email_address")
                    .HasColumnType("ntext");

                entity.Property(e => e.IsDelivered).HasColumnName("isDelivered");

                entity.Property(e => e.IsPaid).HasColumnName("isPaid");

                entity.Property(e => e.OrderStatus).HasColumnName("orderStatus");

                entity.Property(e => e.PaidAt)
                    .HasColumnName("paidAt")
                    .HasColumnType("ntext");

                entity.Property(e => e.PaymentId)
                    .HasColumnName("paymentId")
                    .HasColumnType("ntext");

                entity.Property(e => e.PaypalMethod)
                    .HasColumnName("paypalMethod")
                    .HasMaxLength(30);

                entity.Property(e => e.ShippingPrice).HasColumnName("shippingPrice");

                entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");

                entity.Property(e => e.UpdateTime)
                    .HasColumnName("update_time")
                    .HasColumnType("text");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Orders_Users");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.MenuId })
                    .HasName("orderdetail_pk");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.MenuId).HasColumnName("menuId");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Menus");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Orders");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Calories).HasColumnName("calories");

                entity.Property(e => e.Carb).HasColumnName("carb");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("ntext");

                entity.Property(e => e.Fat).HasColumnName("fat");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasColumnType("ntext");

                entity.Property(e => e.IsDisabled).HasColumnName("isDisabled");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(40);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("money");

                entity.Property(e => e.Protein).HasColumnName("protein");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.TokenId)
                    .HasName("PK__RefreshT__CB3C9E17EF377730");

                entity.ToTable("RefreshToken");

                entity.Property(e => e.TokenId).HasColumnName("token_id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("expiry_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.RefreshTokens)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_RefreshToken_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.IsDisabled).HasColumnName("isDisabled");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(15);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(10);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
