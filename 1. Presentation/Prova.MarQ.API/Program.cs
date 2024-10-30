using Microsoft.EntityFrameworkCore;
using Prova.MarQ.Infra;

var builder = WebApplication.CreateBuilder(args);

// Configurar o DbContext com a string de conex�o
builder.Services.AddDbContext<ProvaMarqDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
