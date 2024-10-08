using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagement.API.Contracts;
using TaskManagement.API.Data;
using TaskManagement.API.Data.Models;
using TaskManagement.API.Data.Repositories;
using TaskManagement.API.Filters;
using TaskManagement.API.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers(o =>
{
    o.Filters.Add(typeof(ExceptionFilter));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<TaskService>();

// Identity configuration
builder.Services.AddIdentity<User, IdentityRole>(o =>
{
    o.Password.RequiredLength = 8;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequireDigit = false;
    o.Password.RequiredUniqueChars = 0;

    o.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<MyDbContext>()
    .AddDefaultTokenProviders();

// Auth configuration
builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config.GetSection("JWT:Issuer").Value,
        ValidAudience = config.GetSection("JWT:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:SecretKey").Value)),

        ValidateActor = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        RequireExpirationTime = true,
    }; 
});

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("AdminOnly", p =>
    p.RequireRole("Admin"));
});

// DbContext configuration
builder.Services.AddDbContext<MyDbContext>(o =>
{
    o.UseSqlServer(config.GetConnectionString("TestDb"));
});

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
