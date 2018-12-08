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

                entity.Property(e => e.IngredientId).HasColumnName("IngredientID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location", "PizzaStore");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order", "PizzaStore");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.PizzaId1).HasColumnName("PizzaID1");

                entity.Property(e => e.PizzaId10).HasColumnName("PizzaID10");

                entity.Property(e => e.PizzaId11).HasColumnName("PizzaID11");

                entity.Property(e => e.PizzaId12).HasColumnName("PizzaID12");

                entity.Property(e => e.PizzaId2).HasColumnName("PizzaID2");

                entity.Property(e => e.PizzaId3).HasColumnName("PizzaID3");

                entity.Property(e => e.PizzaId4).HasColumnName("PizzaID4");

                entity.Property(e => e.PizzaId5).HasColumnName("PizzaID5");

                entity.Property(e => e.PizzaId6).HasColumnName("PizzaID6");

                entity.Property(e => e.PizzaId7).HasColumnName("PizzaID7");

                entity.Property(e => e.PizzaId8).HasColumnName("PizzaID8");

                entity.Property(e => e.PizzaId9).HasColumnName("PizzaID9");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Order__LocationI__6FE99F9F");

                entity.HasOne(d => d.PizzaId1Navigation)
                    .WithMany(p => p.OrderPizzaId1Navigation)
                    .HasForeignKey(d => d.PizzaId1)
                    .HasConstraintName("FK__Order__PizzaID1__71D1E811");

                entity.HasOne(d => d.PizzaId10Navigation)
                    .WithMany(p => p.OrderPizzaId10Navigation)
                    .HasForeignKey(d => d.PizzaId10)
                    .HasConstraintName("FK__Order__PizzaID10__7A672E12");

                entity.HasOne(d => d.PizzaId11Navigation)
                    .WithMany(p => p.OrderPizzaId11Navigation)
                    .HasForeignKey(d => d.PizzaId11)
                    .HasConstraintName("FK__Order__PizzaID11__7B5B524B");

                entity.HasOne(d => d.PizzaId12Navigation)
                    .WithMany(p => p.OrderPizzaId12Navigation)
                    .HasForeignKey(d => d.PizzaId12)
                    .HasConstraintName("FK__Order__PizzaID12__7C4F7684");

                entity.HasOne(d => d.PizzaId2Navigation)
                    .WithMany(p => p.OrderPizzaId2Navigation)
                    .HasForeignKey(d => d.PizzaId2)
                    .HasConstraintName("FK__Order__PizzaID2__72C60C4A");

                entity.HasOne(d => d.PizzaId3Navigation)
                    .WithMany(p => p.OrderPizzaId3Navigation)
                    .HasForeignKey(d => d.PizzaId3)
                    .HasConstraintName("FK__Order__PizzaID3__73BA3083");

                entity.HasOne(d => d.PizzaId4Navigation)
                    .WithMany(p => p.OrderPizzaId4Navigation)
                    .HasForeignKey(d => d.PizzaId4)
                    .HasConstraintName("FK__Order__PizzaID4__74AE54BC");

                entity.HasOne(d => d.PizzaId5Navigation)
                    .WithMany(p => p.OrderPizzaId5Navigation)
                    .HasForeignKey(d => d.PizzaId5)
                    .HasConstraintName("FK__Order__PizzaID5__75A278F5");

                entity.HasOne(d => d.PizzaId6Navigation)
                    .WithMany(p => p.OrderPizzaId6Navigation)
                    .HasForeignKey(d => d.PizzaId6)
                    .HasConstraintName("FK__Order__PizzaID6__76969D2E");

                entity.HasOne(d => d.PizzaId7Navigation)
                    .WithMany(p => p.OrderPizzaId7Navigation)
                    .HasForeignKey(d => d.PizzaId7)
                    .HasConstraintName("FK__Order__PizzaID7__778AC167");

                entity.HasOne(d => d.PizzaId8Navigation)
                    .WithMany(p => p.OrderPizzaId8Navigation)
                    .HasForeignKey(d => d.PizzaId8)
                    .HasConstraintName("FK__Order__PizzaID8__787EE5A0");

                entity.HasOne(d => d.PizzaId9Navigation)
                    .WithMany(p => p.OrderPizzaId9Navigation)
                    .HasForeignKey(d => d.PizzaId9)
                    .HasConstraintName("FK__Order__PizzaID9__797309D9");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Order__UserID__70DDC3D8");
            });

            modelBuilder.Entity<Pizza>(entity =>
            {
                entity.ToTable("Pizza", "PizzaStore");

                entity.Property(e => e.PizzaId).HasColumnName("PizzaID");

                entity.Property(e => e.IngredientId1).HasColumnName("IngredientID1");

                entity.Property(e => e.IngredientId2).HasColumnName("IngredientID2");

                entity.Property(e => e.IngredientId3).HasColumnName("IngredientID3");

                entity.Property(e => e.IngredientId4).HasColumnName("IngredientID4");

                entity.Property(e => e.IngredientId5).HasColumnName("IngredientID5");

                entity.Property(e => e.IngredientId6).HasColumnName("IngredientID6");

                entity.Property(e => e.IngredientId7).HasColumnName("IngredientID7");

                entity.Property(e => e.IngredientId8).HasColumnName("IngredientID8");

                entity.HasOne(d => d.IngredientId1Navigation)
                    .WithMany(p => p.PizzaIngredientId1Navigation)
                    .HasForeignKey(d => d.IngredientId1)
                    .HasConstraintName("FK__Pizza__Ingredien__66603565");

                entity.HasOne(d => d.IngredientId2Navigation)
                    .WithMany(p => p.PizzaIngredientId2Navigation)
                    .HasForeignKey(d => d.IngredientId2)
                    .HasConstraintName("FK__Pizza__Ingredien__6754599E");

                entity.HasOne(d => d.IngredientId3Navigation)
                    .WithMany(p => p.PizzaIngredientId3Navigation)
                    .HasForeignKey(d => d.IngredientId3)
                    .HasConstraintName("FK__Pizza__Ingredien__68487DD7");

                entity.HasOne(d => d.IngredientId4Navigation)
                    .WithMany(p => p.PizzaIngredientId4Navigation)
                    .HasForeignKey(d => d.IngredientId4)
                    .HasConstraintName("FK__Pizza__Ingredien__693CA210");

                entity.HasOne(d => d.IngredientId5Navigation)
                    .WithMany(p => p.PizzaIngredientId5Navigation)
                    .HasForeignKey(d => d.IngredientId5)
                    .HasConstraintName("FK__Pizza__Ingredien__6A30C649");

                entity.HasOne(d => d.IngredientId6Navigation)
                    .WithMany(p => p.PizzaIngredientId6Navigation)
                    .HasForeignKey(d => d.IngredientId6)
                    .HasConstraintName("FK__Pizza__Ingredien__6B24EA82");

                entity.HasOne(d => d.IngredientId7Navigation)
                    .WithMany(p => p.PizzaIngredientId7Navigation)
                    .HasForeignKey(d => d.IngredientId7)
                    .HasConstraintName("FK__Pizza__Ingredien__6C190EBB");

                entity.HasOne(d => d.IngredientId8Navigation)
                    .WithMany(p => p.PizzaIngredientId8Navigation)
                    .HasForeignKey(d => d.IngredientId8)
                    .HasConstraintName("FK__Pizza__Ingredien__6D0D32F4");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "PizzaStore");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.DefaultLocationId).HasColumnName("DefaultLocationID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.DefaultLocation)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.DefaultLocationId)
                    .HasConstraintName("FK__User__DefaultLoc__5AEE82B9");
            });
        }
    }
}
