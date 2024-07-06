using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Pinboard.Application.UseCases;
using Pinboard.DataPersistence;
using Pinboard.Domain.Interfaces.Repositories;
using Pinboard.Domain.Interfaces.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMongoClient>(x =>
    new MongoClient(builder.Configuration.GetSection("PinboardDatabase:ConnectionString").Value));

builder.Services.AddSingleton<IDataContext, DataContext>();

builder.Services.AddSingleton<INoteUseCases, NoteUseCases>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            var origins = builder.Configuration["Cors:AllowedOrigins"]?.Split(";") ?? [];
            policy.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
