using Microsoft.EntityFrameworkCore;
using OnlineSchool.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=school.db"));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();