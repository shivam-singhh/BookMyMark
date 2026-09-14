using BookMyMark.Api.Services;
using BookMyMark.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSingleton<IBookService, BookService>();
builder.Services.AddScoped<IReadingListService, ReadingListService>();
builder.Services.AddDbContext<BookMyMarkDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BookMyMark")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("Ui", policy =>
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Ui");

app.UseAuthorization();

app.MapControllers();

app.Run();
