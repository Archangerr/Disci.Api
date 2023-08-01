using DisciApp.Api.CustomExceptionMiddleware;
using DisciApp.Api.CustomHealthCheck;
using DisciApp.Api.DataBaseContext;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using HealthChecks.UI.Client;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ImgDbContext>(options =>
    options.UseSqlServer("Data Source=DESKTOP-KK8B4QE\\SQLEXPRESS01;Initial Catalog=DisciApp;User id=sa;Password=1234qqqQ;TrustServerCertificate=True"));

builder.Services.AddDbContext<RezervasyonDbContext>(options =>
    options.UseSqlServer("Data Source=DESKTOP-KK8B4QE\\SQLEXPRESS01;Initial Catalog=DisciApp;User id=sa;Password=1234qqqQ;TrustServerCertificate=True"));
builder.Services
.AddHealthChecks()
    .AddCheck<CustomHealthCheck>(nameof(CustomHealthCheck));

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
