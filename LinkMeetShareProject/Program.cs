using System.Text;
using LinkMeetShareProject;
using LinkMeetShareProject.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dbPath = Path.Combine(AppContext.BaseDirectory, "LinkMeetShareProject.db");
builder.Services.AddDbContext<LinkMeetShareProjectDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddIdentityCore<ApiUser>()
    .AddRoles<IdentityRole>().AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("HotelListingApi")
    .AddEntityFrameworkStores<LinkMeetShareProjectDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<UserDtoMapper>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])) 

    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", cpb => cpb.AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin());
    //cpb = configure Policy builder
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication(); 

app.UseRouting();

app.UseAuthorization();


app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var servicesProvider = scope.ServiceProvider;
    using (var context = servicesProvider.GetRequiredService<LinkMeetShareProjectDbContext>())
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        try
        {
            // Add test user
            var userManager = servicesProvider.GetRequiredService<UserManager<ApiUser>>();
            var testUser = new ApiUser
            {
                UserName = "test@example.com",
                Email = "test@example.com",
                FirstName = "Test",
                LastName = "User"
            };

            var result = await userManager.CreateAsync(testUser, "Test123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(testUser, "User");
            }
        }
        catch (Exception ex)
        {
            // Log the error
            var logger = servicesProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}



app.Run();
