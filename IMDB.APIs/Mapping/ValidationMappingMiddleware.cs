using FluentValidation;
using IMDB.Contracts.Responses;

namespace IMDB.APIs.Mapping;

public class ValidationMappingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = 400;
            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = ex.Errors.Select(x => new ValidationResponse
                {
                    Name = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };
            await context.Response.WriteAsJsonAsync(validationFailureResponse);
        }
    }
}