using Microsoft.EntityFrameworkCore;
using WebAPI.Database;
using WebAPI.ServiceContracts;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITravelingSalesmanService, TravelingSalesmanService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    string? connectionStringTemplate = builder.Configuration.GetConnectionString("DockerDb");
    string? password = Environment.GetEnvironmentVariable("MSSQL_SA_PASSWORD");
    string connectionString = string.Format(connectionStringTemplate, password);
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
