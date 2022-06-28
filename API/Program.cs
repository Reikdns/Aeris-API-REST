using Domain.Repository;
using Infrastructure.Helpers;
using Infrastructure.Repository;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Aeris API",
                    Description = "API para la aplicación web Aeris",
                    TermsOfService = new Uri("https://cla.dotnetfoundation.org/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Reinaldo José Durán Navarro",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/Reikdns"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Licencia dotnet foundation",
                        Url = new Uri("https://www.byasystems.co/license"),
                    }
                });
            });
var appSettingSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingSection);
var appSettings = appSettingSection.Get<AppSettings>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
