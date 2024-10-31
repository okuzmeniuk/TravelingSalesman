using Application.Database;
using Application.ServiceContracts;
using Application.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ITravelingSalesmanService, TravelingSalesmanService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string? connectionStringTemplate = builder.Configuration.GetConnectionString("DockerDb");
    string? password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
    string connectionString = string.Format(connectionStringTemplate, password);
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
