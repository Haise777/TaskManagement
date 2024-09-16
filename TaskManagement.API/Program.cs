using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Models;
using TaskManagement.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<AuthService>();

// Identity configuration
builder.Services.AddIdentity<IdentityUser, IdentityRole>(o =>
{
    o.Password.RequiredLength = 8;
    o.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<MyDbContext>()
    .AddDefaultTokenProviders();

// DbContext configuration
builder.Services.AddDbContext<MyDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("TestDb"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
