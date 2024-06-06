using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

public class JwtExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtExceptionHandler> _logger;

    public JwtExceptionHandler(RequestDelegate next, ILogger<JwtExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.LogError(ex, "Token has expired");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Token has expired.");
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogError(ex, "Token is invalid");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Token is invalid.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = new { message = ex.Message };
            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
