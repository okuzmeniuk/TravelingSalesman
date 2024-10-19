using Microsoft.EntityFrameworkCore;
using WebAPI.Database;
using WebAPI.ServiceContracts;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITravelingSalesmanService, TravelingSalesmanService>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();
var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
