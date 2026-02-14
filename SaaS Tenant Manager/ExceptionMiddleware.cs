//namespace Presentation
//{
//    using System.Net;
//    using System.Text.Json;

//    public class ExceptionMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<ExceptionMiddleware> _logger;

//    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
//    {
//        _next = next;
//        _logger = logger;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "An error occurred in the system");
//            await HandleExceptionAsync(context, ex);
//        }
//    }

//    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
//    {
//        context.Response.ContentType = "application/json";
//        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//            var response = ApiResponse.Failure(new List<string>());
//        if (exception is FluentValidation.ValidationException validationResult)
//            {
//                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
//            }
//        else
//            {
//                response.Errors = new List<string> { exception.Message };
//            }
//        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
//        return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
//    }
//}
//}
