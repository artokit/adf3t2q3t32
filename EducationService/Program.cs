using System.Reflection;
using Common;
using Common.Interfaces;
using Database;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EducationService.Repositories;
using EducationService.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

//существуют со старта приложения Singleton
builder.Services.AddSingleton<Connection>();
builder.Services.AddSingleton<IConfigurationSettings, ConfigurationSettings>();

//создаются каждый http запрос Scoped
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthorizationService>();


//создаются раз когда вызываются Transident


builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb.AddPostgres().WithGlobalConnectionString(connectionString).ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
    .AddLogging(rb => rb.AddFluentMigratorConsole());

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

app.UseHttpsRedirection();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();

app.Run();
