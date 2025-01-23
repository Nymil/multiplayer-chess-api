using System.Net;
using Logic.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MultiplayerChessApi.Mapping;
using MultiplayerChessApi.Middleware;
using MultiplayerChessApi.SchemaFilters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<NonNullableSchemaFilter>();
});

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(ChessGameProfile));

// Register ChessApiService as a singleton
builder.Services.AddSingleton<IChessApiService>(ChessApiService.Instance);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
