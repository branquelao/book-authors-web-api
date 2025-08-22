using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;
using WebSQLCRUD.Data;
using WebSQLCRUD.Helpers;
using WebSQLCRUD.Models;
using WebSQLCRUD.Services;
using WebSQLCRUD.Services.Author;
using WebSQLCRUD.Services.Book;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddHostedService<DatabaseHelper>();
builder.Services.AddHostedService<BackgroundHelper>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthorInterface, AuthorService>(); 
builder.Services.AddScoped<IBookInterface, BookService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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