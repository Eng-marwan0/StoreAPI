using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;
namespace StoreAPI.Data;
public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<AdminLog> AdminLogs { get; set; }
    public virtual DbSet<AdminUser> AdminUsers { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<CartItem> CartItems { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Clinic> Clinics { get; set; }
    public virtual DbSet<Coupon> Coupons { get; set; }
    public virtual DbSet<CouponUsage> CouponUsages { get; set; }
    public virtual DbSet<Doctor> Doctors { get; set; }
    public virtual DbSet<DoctorSchedule> DoctorSchedules { get; set; }
    public virtual DbSet<OTP> OTPs { get; set; }
    public virtual DbSet<Offer> Offers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<ProductImage> ProductImages { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Shipment> Shipments { get; set; }
    public virtual DbSet<ShippingCompany> ShippingCompanies { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserAddress> UserAddresses { get; set; }
    public virtual DbSet<WishlistItem> WishlistItems { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.Property(e => e.PriceAtTime)
                .HasPrecision(10, 2); // 👈 تحديد نوع decimal(10,2)

            modelBuilder.Entity<WishlistItem>(entity =>
            {
                entity.HasKey(e => e.WishlistItemId).HasName("PK_WishlistItems");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(sysutcdatetime())");

                entity.HasIndex(e => new { e.UserId, e.ProductId })
                    .IsUnique()
                    .HasDatabaseName("UQ_Wishlist_User_Product");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WishlistItems)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WishlistItems_Users");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.WishlistItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WishlistItems_Products");
            });

        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.Property(e => e.ShippingCost)
                .HasPrecision(10, 2); // 👈 تحديد نوع decimal(10,2)
        });

        modelBuilder.Entity<AdminLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__AdminLog__5E548648BBC543DB");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.HasOne(d => d.Admin).WithMany(p => p.AdminLogs).HasConstraintName("FK_AdminLogs_AdminUsers");
        });

        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__AdminUse__719FE488929B46A5");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasMany(d => d.Roles).WithMany(p => p.Admins)
                .UsingEntity<Dictionary<string, object>>(
                    "AdminUserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AdminUserRoles_Roles"),
                    l => l.HasOne<AdminUser>().WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AdminUserRoles_AdminUsers"),
                    j =>
                    {
                        j.HasKey("AdminId", "RoleId");
                        j.ToTable("AdminUserRoles");
                    });
        });
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__73951AED05B6916C");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.HasOne(d => d.Clinic).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Clinics");
            entity.HasOne(d => d.Doctor).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Doctors");
            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bookings_Users");
        });
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Carts__51BCD7B7303578F0");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.HasOne(d => d.User).WithOne(p => p.Cart)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carts_Users");
        });
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__488B0B0A0F973C79");
            entity.Property(e => e.TotalPrice).HasComputedColumnSql("([Quantity]*[UnitPrice])", true);
            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Carts");
            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Products");
        });
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B38CB1BCB");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK_Categories_Parent");
        });
        modelBuilder.Entity<Clinic>(entity =>
        {
            entity.HasKey(e => e.ClinicId).HasName("PK__Clinics__3347C2DD1C08433B");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });
        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.CouponId).HasName("PK__Coupons__384AF1BA2232599D");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });
        modelBuilder.Entity<CouponUsage>(entity =>
        {
            entity.HasKey(e => e.CouponUsageId).HasName("PK__CouponUs__10AE105D5AFA5B04");
            entity.Property(e => e.UsedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.HasOne(d => d.Coupon).WithMany(p => p.CouponUsages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CouponUsages_Coupons");
            entity.HasOne(d => d.Order).WithMany(p => p.CouponUsages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CouponUsages_Orders");
            entity.HasOne(d => d.User).WithMany(p => p.CouponUsages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CouponUsages_Users");
        });
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EBF7DD906CE");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasOne(d => d.Clinic).WithMany(p => p.Doctors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Doctors_Clinics");
        });
        modelBuilder.Entity<DoctorSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__DoctorSc__9C8A5B498198651B");
            entity.Property(e => e.SlotDurationMinutes).HasDefaultValue(15);
            entity.HasOne(d => d.Doctor).WithMany(p => p.DoctorSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DoctorSchedules_Doctors");
        });

        modelBuilder.Entity<OTP>(entity =>
        {
            entity.HasKey(e => e.OtpId).HasName("PK__OTP__3143C4A30E10E75C");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });
        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.OfferId).HasName("PK__Offers__8EBCF09177A91712");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasMany(d => d.Products).WithMany(p => p.Offers)
                .UsingEntity<Dictionary<string, object>>(
                    "OfferProduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_OfferProducts_Products"),
                    l => l.HasOne<Offer>().WithMany()
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_OfferProducts_Offers"),
                    j =>
                    {
                        j.HasKey("OfferId", "ProductId");
                        j.ToTable("OfferProducts");
                    });
        });
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF38BC5CF5");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.HasOne(d => d.Coupon).WithMany(p => p.Orders).HasConstraintName("FK_Orders_Coupons");
            entity.HasOne(d => d.DeliveryAddress).WithMany(p => p.Orders).HasConstraintName("FK_Orders_Addresses");
            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED068188F4C995");
            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Orders");
            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Products");
        });
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CDFC4CD2FD");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");
        });
        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ProductI__7516F70CACD2902B");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductImages_Products");
        });
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AB73A0D98");
        });
        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.ShipmentId).HasName("PK__Shipment__5CAD37ED11B68E1E");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.HasOne(d => d.Order).WithMany(p => p.Shipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shipments_Orders");
            entity.HasOne(d => d.ShippingCompany).WithMany(p => p.Shipments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shipments_ShippingCompanies");
        });
        modelBuilder.Entity<ShippingCompany>(entity =>
        {
            entity.HasKey(e => e.ShippingCompanyId).HasName("PK__Shipping__DADC8FBA61A402ED");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CEF6CFF0E");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__UserAddr__091C2AFB8076F283");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAddresses_Users");
        });
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}