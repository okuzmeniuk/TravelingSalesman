using WebAPI.ServiceContracts;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITravelingSalesmanService, TravelingSalesmanService>();

builder.Services.AddControllers();
var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
