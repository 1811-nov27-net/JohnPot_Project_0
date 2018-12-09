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
                    .HasName("UQ__Ingredie__737584F630263E18")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
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
                entity.ToTable("Order", "PizzaStore");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Order__LocationI__3A179ED3");

                entity.HasOne(d => d.PizzaId1Navigation)
                    .WithMany(p => p.OrderPizzaId1Navigation)
                    .HasForeignKey(d => d.PizzaId1)
                    .HasConstraintName("FK__Order__PizzaId1__3BFFE745");

                entity.HasOne(d => d.PizzaId10Navigation)
                    .WithMany(p => p.OrderPizzaId10Navigation)
                    .HasForeignKey(d => d.PizzaId10)
                    .HasConstraintName("FK__Order__PizzaId10__44952D46");

                entity.HasOne(d => d.PizzaId11Navigation)
                    .WithMany(p => p.OrderPizzaId11Navigation)
                    .HasForeignKey(d => d.PizzaId11)
                    .HasConstraintName("FK__Order__PizzaId11__4589517F");

                entity.HasOne(d => d.PizzaId12Navigation)
                    .WithMany(p => p.OrderPizzaId12Navigation)
                    .HasForeignKey(d => d.PizzaId12)
                    .HasConstraintName("FK__Order__PizzaId12__467D75B8");

                entity.HasOne(d => d.PizzaId2Navigation)
                    .WithMany(p => p.OrderPizzaId2Navigation)
                    .HasForeignKey(d => d.PizzaId2)
                    .HasConstraintName("FK__Order__PizzaId2__3CF40B7E");

                entity.HasOne(d => d.PizzaId3Navigation)
                    .WithMany(p => p.OrderPizzaId3Navigation)
                    .HasForeignKey(d => d.PizzaId3)
                    .HasConstraintName("FK__Order__PizzaId3__3DE82FB7");

                entity.HasOne(d => d.PizzaId4Navigation)
                    .WithMany(p => p.OrderPizzaId4Navigation)
                    .HasForeignKey(d => d.PizzaId4)
                    .HasConstraintName("FK__Order__PizzaId4__3EDC53F0");

                entity.HasOne(d => d.PizzaId5Navigation)
                    .WithMany(p => p.OrderPizzaId5Navigation)
                    .HasForeignKey(d => d.PizzaId5)
                    .HasConstraintName("FK__Order__PizzaId5__3FD07829");

                entity.HasOne(d => d.PizzaId6Navigation)
                    .WithMany(p => p.OrderPizzaId6Navigation)
                    .HasForeignKey(d => d.PizzaId6)
                    .HasConstraintName("FK__Order__PizzaId6__40C49C62");

                entity.HasOne(d => d.PizzaId7Navigation)
                    .WithMany(p => p.OrderPizzaId7Navigation)
                    .HasForeignKey(d => d.PizzaId7)
                    .HasConstraintName("FK__Order__PizzaId7__41B8C09B");

                entity.HasOne(d => d.PizzaId8Navigation)
                    .WithMany(p => p.OrderPizzaId8Navigation)
                    .HasForeignKey(d => d.PizzaId8)
                    .HasConstraintName("FK__Order__PizzaId8__42ACE4D4");

                entity.HasOne(d => d.PizzaId9Navigation)
                    .WithMany(p => p.OrderPizzaId9Navigation)
                    .HasForeignKey(d => d.PizzaId9)
                    .HasConstraintName("FK__Order__PizzaId9__43A1090D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Order__UserId__3B0BC30C");
            });

            modelBuilder.Entity<Pizza>(entity =>
            {
                entity.ToTable("Pizza", "PizzaStore");

                entity.HasOne(d => d.IngredientId1Navigation)
                    .WithMany(p => p.PizzaIngredientId1Navigation)
                    .HasForeignKey(d => d.IngredientId1)
                    .HasConstraintName("FK__Pizza__Ingredien__308E3499");

                entity.HasOne(d => d.IngredientId2Navigation)
                    .WithMany(p => p.PizzaIngredientId2Navigation)
                    .HasForeignKey(d => d.IngredientId2)
                    .HasConstraintName("FK__Pizza__Ingredien__318258D2");

                entity.HasOne(d => d.IngredientId3Navigation)
                    .WithMany(p => p.PizzaIngredientId3Navigation)
                    .HasForeignKey(d => d.IngredientId3)
                    .HasConstraintName("FK__Pizza__Ingredien__32767D0B");

                entity.HasOne(d => d.IngredientId4Navigation)
                    .WithMany(p => p.PizzaIngredientId4Navigation)
                    .HasForeignKey(d => d.IngredientId4)
                    .HasConstraintName("FK__Pizza__Ingredien__336AA144");

                entity.HasOne(d => d.IngredientId5Navigation)
                    .WithMany(p => p.PizzaIngredientId5Navigation)
                    .HasForeignKey(d => d.IngredientId5)
                    .HasConstraintName("FK__Pizza__Ingredien__345EC57D");

                entity.HasOne(d => d.IngredientId6Navigation)
                    .WithMany(p => p.PizzaIngredientId6Navigation)
                    .HasForeignKey(d => d.IngredientId6)
                    .HasConstraintName("FK__Pizza__Ingredien__3552E9B6");

                entity.HasOne(d => d.IngredientId7Navigation)
                    .WithMany(p => p.PizzaIngredientId7Navigation)
                    .HasForeignKey(d => d.IngredientId7)
                    .HasConstraintName("FK__Pizza__Ingredien__36470DEF");

                entity.HasOne(d => d.IngredientId8Navigation)
                    .WithMany(p => p.PizzaIngredientId8Navigation)
                    .HasForeignKey(d => d.IngredientId8)
                    .HasConstraintName("FK__Pizza__Ingredien__373B3228");
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
                    .HasConstraintName("FK__User__DefaultLoc__2AD55B43");
            });
        }
    }
}
