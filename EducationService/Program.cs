using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EducationService;
using EducationService.Domain;
using EducationService.Repositories;
using EducationService.Repositories.Interfaces;
using EducationService.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddControllers();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<DatabaseConnection>();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentMigratorCore().ConfigureRunner(rb => rb.AddPostgres().WithGlobalConnectionString(connectionString).ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()).AddLogging(rb => rb.AddFluentMigratorConsole());
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = AuthOptions.Issuer,
        ValidAudience = AuthOptions.Audience,
        ValidateLifetime = true,
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true
    };
});

var app = builder.Build();
var serviceProvider = app.Services.CreateScope().ServiceProvider;
var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.Run();
