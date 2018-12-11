using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PizzaStoreData.DataAccess
{
    public partial class PizzaStoreContext : DbContext
    {
        public PizzaStoreContext()
        {
        }

        public PizzaStoreContext(DbContextOptions<PizzaStoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Pizza> Pizza { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("Ingredient", "PizzaStore");

                entity.HasIndex(e => e.Name)
                    .HasName("UQ__Ingredie__737584F64DCDCAA9")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => new { e.LocationId, e.IngredientId });

                entity.ToTable("Inventory", "PizzaStore");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__Ingre__65C116E7");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Inventory)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__Locat__64CCF2AE");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location", "PizzaStore");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.PizzaId });

                entity.ToTable("Order", "PizzaStore");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Order__LocationI__6E565CE8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Order__UserId__6F4A8121");
            });

            modelBuilder.Entity<Pizza>(entity =>
            {
                entity.HasKey(e => new { e.PizzaId, e.IngredientId });

                entity.ToTable("Pizza", "PizzaStore");

                entity.HasOne(d => d.Ingredient)
                    .WithMany(p => p.Pizza)
                    .HasForeignKey(d => d.IngredientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Pizza__Ingredien__6B79F03D");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "PizzaStore");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.DefaultLocation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.DefaultLocationId)
                    .HasConstraintName("FK__User__DefaultLoc__689D8392");
            });
        }
    }
}
