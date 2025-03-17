using MedicalAPI.Models;
using MedicalAPI.Repositories;
using MedicalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
           policy =>
           {
               policy.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod();
           });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<MedicalAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//var elasticUsername = "elastic"; // Elasticsearch username
//var elasticPassword = "Informatica26?";
//var elasticUri = "https://localhost:9200";
builder.Services.AddSingleton(new ElasticSearchService("https://localhost:9200", "elastic", "Informatica26?"));

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IRepository<Role>, RolesRepository>();
builder.Services.AddScoped<IReportsRepository<Report>, ReportsRepository>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<PatientsService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseStaticFiles();


app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
