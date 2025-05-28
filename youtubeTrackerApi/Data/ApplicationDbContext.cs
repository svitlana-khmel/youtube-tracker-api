using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Ensure Identity is correctly configured
        builder.Entity<WeatherForecast>().HasNoKey(); // Mark it as a keyless entity
    }
}





// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;

// public class ApplicationDbContext : IdentityDbContext<IdentityUser>
// {
//     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
//         base(options)
//     { }
// }


// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using EyeTrackingApi.Models;

// namespace EyeTrackingApi.Data;

// public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
// {
//     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//         : base(options)
//     {
//     }

//     public DbSet<WeatherForecast> WeatherForecasts { get; set; }

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         base.OnModelCreating(modelBuilder);

//         modelBuilder.Entity<WeatherForecast>(entity =>
//         {
//             entity.HasKey(e => e.Id);
//             entity.Property(e => e.Summary).HasMaxLength(100);
//         });
//     }
// }




// using Microsoft.AspNetCore.Identity.Data; //EntityFrameworkCore
// using Microsoft.EntityFrameworkCore;


// namespace EyeTrackingApi.Data
// {
//     public class ApplicationDbContext : IdentityDbContext
//     {
//         public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//             : base(options)
//         {
//         }

//         public DbSet<EyeTrackingData> EyeTrackingData { get; set; }
//         public DbSet<TrackingData> TrackingData { get; set; }

//         protected override void OnModelCreating(ModelBuilder modelBuilder)
//         {
//             base.OnModelCreating(modelBuilder);

//             modelBuilder.Entity<EyeTrackingData>(entity =>
//             {
//                 entity.HasKey(e => e.PageUrl);
//                 entity.Property(e => e.VideoTitle).IsRequired();
//                 entity.Property(e => e.StartTime).IsRequired();
//                 entity.HasMany(e => e.TrackingData)
//                       .WithOne()
//                       .OnDelete(DeleteBehavior.Cascade);
//             });

//             modelBuilder.Entity<TrackingData>(entity =>
//             {
//                 entity.HasKey(e => e.Timestamp);
//                 entity.OwnsOne(e => e.VideoState);
//                 entity.OwnsOne(e => e.GazeData);
//             });
//         }
//     }
// }



// public class ApplicationUser : IdentityUser
// {
//     // Add any additional user properties here if needed
// }