namespace AspnetCoreMvcFull.Core.Context;

using AspnetCoreMvcFull.Models;
using AspnetCoreMvcFull.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
  public DbSet<UserGroup> Groups { get; set; }
  public DbSet<Product> Products { get; set; }
  public DbSet<Consumption> Consumptions { get; set; }

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
  public ApplicationDbContext() { }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    // Seed Data


    builder.Entity<Consumption>()
    .HasOne(c => c.User)
    .WithMany()
    .HasForeignKey(c => c.UserId)
    .IsRequired();

    builder.Entity<Consumption>()
    .HasOne(c => c.Product)
    .WithMany()
    .HasForeignKey(c => c.ProductId)
    .IsRequired();

    builder.Entity<UserGroup>().HasData(
      new UserGroup { Id = 1, Name = "Milk" },
      new UserGroup { Id = 2, Name = "Yogurt" }
    );

    builder.Entity<Product>().HasData(
        new Product { Id = 1, Name = "Milk1", GroupId = 1 },
        new Product { Id = 2, Name = "Milk2", GroupId = 1 },
        new Product { Id = 3, Name = "Yogurt1", GroupId = 2 },
        new Product { Id = 4, Name = "Yogurt2", GroupId = 2 },
        new Product { Id = 5, Name = "Yogurt13", GroupId = 2 }
    );
  }
}
