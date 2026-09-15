using System.Text.Json.Serialization;
using BookMyMark.AppService.Interfaces;
using BookMyMark.AppService.Services;
using BookMyMark.Command.ReadingList;
using BookMyMark.Infrastructure.Data;
using BookMyMark.Infrastructure.Repositories;
using BookMyMark.Infrastructure.Services;
using BookMyMark.Query.Books;
using BookMyMark.Query.ReadingList;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSingleton<IBookCatalogService, BookCatalogService>();
builder.Services.AddSingleton<IBookAppService, BookAppService>();
builder.Services.AddScoped<IReadingListRepository, ReadingListRepository>();
builder.Services.AddScoped<IReadingListAppService, ReadingListAppService>();

builder.Services.AddScoped<GetBooksQueryHandler>();
builder.Services.AddScoped<GetBookByIdQueryHandler>();
builder.Services.AddScoped<GetReadingListQueryHandler>();
builder.Services.AddScoped<GetReadingListItemQueryHandler>();
builder.Services.AddScoped<AddReadingListItemCommandHandler>();
builder.Services.AddScoped<UpdateReadingStatusCommandHandler>();
builder.Services.AddScoped<DeleteReadingListItemCommandHandler>();

builder.Services.AddDbContext<BookMyMarkDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("BookMyMark")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Ui", policy =>
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
