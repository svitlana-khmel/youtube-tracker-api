
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Authorization
builder.Services.AddAuthorization();

// Configure identity database
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseInMemoryDatabase("AppDb"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Ensure Identity is only added once
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IEmailSender<IdentityUser>, NoOpEmailSender>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ensure middleware is only added once
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();



app.MapIdentityApi<IdentityUser>();

app.Run();



// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;

// var builder = WebApplication.CreateBuilder(args);

// // Configure in-memory database
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseInMemoryDatabase("AppDb"));

// // Configure Identity
// builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//     .AddEntityFrameworkStores<ApplicationDbContext>()
//     .AddDefaultTokenProviders();

// // Add authentication & authorization
// builder.Services.AddAuthorization();
// builder.Services.AddAuthentication();

// // Enable Identity API endpoints
// builder.Services.AddIdentityApiEndpoints<IdentityUser>();

// // Configure Swagger
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// var app = builder.Build();

// // Enable Swagger UI in Development mode
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// // Enable security middleware
// app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();

// // Weather data summaries
// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// // Register Weather Forecast API
// app.MapGet("/weatherforecast", (ApplicationDbContext db) =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         )).ToList();

//     // Store in DB
//     db.WeatherForecasts.AddRange(forecast);
//     db.SaveChanges();

//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi()
// .RequireAuthorization();

// //app.MapIdentityApi<IdentityUser>();

// app.Run();

// Define ApplicationDbContext
// public class ApplicationDbContext : IdentityDbContext<IdentityUser>
// {
//     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

//     public DbSet<WeatherForecast> WeatherForecasts { get; set; }
// }

// Define WeatherForecast Model
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


//**************************

// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using System.Text;
// using Microsoft.OpenApi.Models;
// using EyeTrackingApi.Services;

// using EyeTrackingApi.Models;
// using Microsoft.AspNetCore.Mvc;


// var builder = WebApplication.CreateBuilder(args);

// // Add services
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();

// // Configure Swagger
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Eye Tracking API", Version = "v1" });

//     // Add JWT Authentication to Swagger
//     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Description = "JWT Authorization header using the Bearer scheme",
//         Type = SecuritySchemeType.Http,
//         Scheme = "bearer"
//     });
//     c.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type = ReferenceType.SecurityScheme,
//                     Id = "Bearer"
//                 }
//             },
//             new string[] {}
//         }
//     });
// });

// // builder.Services.AddDbContext<ApplicationDbContext>(options =>
// //     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// // // Configure Identity
// // builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
// //     .AddEntityFrameworkStores<ApplicationDbContext>()
// //     .AddDefaultTokenProviders();

// // builder.Services.AddAuthentication(options =>
// // {
// //     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
// //     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// // })
// // .AddJwtBearer(options =>
// // {
// //     options.TokenValidationParameters = new TokenValidationParameters
// //     {
// //         ValidateIssuer = true,
// //         ValidateAudience = true,
// //         ValidateLifetime = true,
// //         ValidateIssuerSigningKey = true,
// //         ValidIssuer = builder.Configuration["Jwt:Issuer"],
// //         ValidAudience = builder.Configuration["Jwt:Audience"],
// //         IssuerSigningKey = new SymmetricSecurityKey(
// //             Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
// //     };
// // });

// builder.Services.AddDbContext<ApplicationDbContext>(
//     options => options.UseInMemoryDatabase("AppDb"));

// builder.Services.AddScoped<IReportService, ReportService>();

// // Add Authorization
// builder.Services.AddAuthorization();
// builder.Services.AddIdentityApiEndpoints<IdentityUser>()
//     .AddEntityFrameworkStores<ApplicationDbContext>();

// var app = builder.Build();

// app.MapIdentityApi<IdentityUser>();

// // Configure the HTTP request pipeline
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();

// // Map Controllers
// app.MapControllers();
// app.MapSwagger().RequireAuthorization();

// app.Run();



// app.MapGet("/weatherforecast", () =>
// {
//     var summaries = new[]
// {
//         "Freezing", "Bracing", "Chilly", "Cool", "Mild",
//         "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//     };

//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi()
// .RequireAuthorization();



// app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager,
//     [FromBody] object empty) =>
// {
//     if (empty != null)
//     {
//         await signInManager.SignOutAsync();
//         return Results.Ok();
//     }
//     return Results.Unauthorized();
// })
// .WithOpenApi()
// .RequireAuthorization();


// static string GetRandomSummary()
// {
//     var summaries = new[]
//     {
//         "Freezing", "Bracing", "Chilly", "Cool", "Mild",
//         "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//     };

//     return summaries[Random.Shared.Next(summaries.Length)];
// }



// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.OpenApi.Models;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using Microsoft.IdentityModel.Tokens;
// using EyeTrackingApi.Data;


// var builder = WebApplication.CreateBuilder(args);

// // Add services
// builder.Services.AddControllers();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Eye Tracking API", Version = "v1" });
//     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         Type = SecuritySchemeType.Http,
//         Scheme = "bearer",
//         BearerFormat = "JWT"
//     });
// });

// // Add Identity and JWT authentication
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//     .AddEntityFrameworkStores<ApplicationDbContext>()
//     .AddDefaultTokenProviders();

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = true,
//         ValidateAudience = true,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         ValidIssuer = builder.Configuration["Jwt:Issuer"],
//         ValidAudience = builder.Configuration["Jwt:Audience"],
//         IssuerSigningKey = new SymmetricSecurityKey(
//             Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//     };
// });

// builder.Services.AddTransient<IReportService, ReportService>();

// var app = builder.Build();

// // Configure middleware
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();
// app.UseAuthentication();
// app.UseAuthorization();
// app.MapControllers();
// app.Run();



















// using EyeTrackingApi.Data;
// using Microsoft.EntityFrameworkCore;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// builder.Services.AddAuthorization();
// builder.Services.AddIdentityApiEndpoints<IdentityUser>()
//     .AddEntityFrameforkStores<DataContext>();

// AppContext.MapIdentityApi<IdentityUser>()

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

// app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }
