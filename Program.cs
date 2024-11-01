using dotnet_github_webhook;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<Context>(options => options.UseSqlite(@"Data Source=Data\Database.db"));

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();