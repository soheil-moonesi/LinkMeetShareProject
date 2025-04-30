using LinkMeetShareProject;
using LinkMeetShareProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", cpb => cpb.AllowAnyHeader().AllowAnyHeader().AllowAnyOrigin());
    //cpb = configure Policy builder
});

var dbPath = Path.Combine(AppContext.BaseDirectory, "LinkMeetShareProject.db");
builder.Services.AddDbContext<LinkMeetShareProjectDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));


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
        try
        {

                context.Update(new MeetingLink()
                {
                    MeetingLinkKey = 1,
                    Link = "www.soheil.com",
                    Tittle = "soheil Moonesi",
                });

                context.SaveChanges();

                context.Update(new User
                {
                    UserKey = 1,
                    Email = "soheil@gmail.com"
                });

                context.Update(new MeetingLinkUser()
                {
                    MeetingLinkKey_R = 1,
                    UserKey_R = 1
                });

                context.SaveChanges();
           
        }
        catch (Exception ex)
        {
            throw;
        }


    }
}



app.Run();
