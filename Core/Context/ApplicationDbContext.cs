namespace AspnetCoreMvcFull.Core.Context;

using AspnetCoreMvcFull.Models;
using AspnetCoreMvcFull.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
  public DbSet<UserGroup> Groups { get; set; }
  public DbSet<Product> Products { get; set; }
  public DbSet<Consumption> Consumptions { get; set; }

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    // Seed Data
    builder.Entity<UserGroup>().HasData(
        new UserGroup { Id = 1, Name = "شیر" },
        new UserGroup { Id = 2, Name = "ماست" }
    );

    builder.Entity<Consumption>()
    .HasOne(c => c.User)
    .WithMany()
    .HasForeignKey(c => c.UserId)
    .IsRequired();
    /*    builder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "شیر کم‌چرب", GroupId = 1 },
            new Product { Id = 2, Name = "شیر پرچرب", GroupId = 1 },
            new Product { Id = 3, Name = "ماست ساده", GroupId = 2 },
            new Product { Id = 4, Name = "ماست میوه‌ای", GroupId = 2 },
            new Product { Id = 5, Name = "ماست یونانی", GroupId = 2 }
        );*/
  }
}
