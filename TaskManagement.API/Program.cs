using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagement.API.Models;
using TaskManagement.API.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AuthService>();

// Identity configuration
builder.Services.AddIdentity<IdentityUser, IdentityRole>(o =>
{
    o.Password.RequiredLength = 8;
    o.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<MyDbContext>()
    .AddDefaultTokenProviders();

// Auth configuration
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
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

// DbContext configuration
builder.Services.AddDbContext<MyDbContext>(o =>
{
    o.UseSqlServer(config.GetConnectionString("TestDb"));
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
