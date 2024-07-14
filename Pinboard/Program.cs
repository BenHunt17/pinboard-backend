using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Pinboard.Application.UseCases;
using Pinboard.DataPersistence;
using Pinboard.Domain.Interfaces;
using Pinboard.Domain.Interfaces.Repositories;
using Pinboard.Domain.Interfaces.UseCases;
using Pinboard.Middleware;
using Pinboard.RestApi.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth0:Domain"];
        options.Audience = builder.Configuration["Auth0:Audience"];
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
        }
        });
});

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserState, UserState>();

builder.Services.AddTransient<ErrorHandlerMiddleware>();

builder.Services.AddSingleton<IMongoClient>(x =>
    new MongoClient(builder.Configuration.GetSection("PinboardDatabase:ConnectionString").Value));

builder.Services.AddSingleton<IDataContext, DataContext>();

builder.Services.AddSingleton<IAuthService, AuthService>();

builder.Services.AddScoped<INoteUseCases, NoteUseCases>();
builder.Services.AddScoped<IUserUseCases, UserUseCases>();

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

app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization();

app.Run();
