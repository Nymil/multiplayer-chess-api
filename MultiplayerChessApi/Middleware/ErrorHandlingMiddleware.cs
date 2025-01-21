using System;
using System.Text.Json;
using Logic.Domain.Exceptions;
using MultiplayerChessApi.Response;

namespace MultiplayerChessApi.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ChessGameException ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, ChessGameException exception)
    {
        HttpResponse response = context.Response;
        response.ContentType = "application/json";

        ErrorResponse errorResponse = new ErrorResponse(exception.Message, exception.Code);

        response.StatusCode = exception.Code;

        string jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return response.WriteAsync(jsonResponse);
    }
}
