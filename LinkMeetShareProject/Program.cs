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
       // todo: create process to stop when seed is done before
            //context.Add(new MeetingLink()
            //{
            //    MeetingLinkKey = 1,
            //    Link = "www.soheil.com",
            //    Tittle = "soheil Moonesi",
            //});

            //context.SaveChanges();

            //context.Add(new User
            //{
            //    UserKey = 1,
            //    Email = "soheil@gmail.com"
            //});

            //context.Add(new MeetingLinkUser()
            //{
            //    MeetingLinkKey_R = 1,
            //    UserKey_R = 1
            //});

            //context.SaveChanges();


            //context.Add(new MeetingLink()
            //{
            //    MeetingLinkKey = 2,
            //    Link = "www.soh.com",
            //    Tittle = "soh",
            //});

            //context.SaveChanges();

            //context.Add(new MeetingLink()
            //{
            //    MeetingLinkKey = 3,
            //    Link = "www.face.com",
            //    Tittle = "face",
            //});

            //context.SaveChanges();


        }
        catch (Exception ex)
        {
            throw;
        }


    }
}



app.Run();
