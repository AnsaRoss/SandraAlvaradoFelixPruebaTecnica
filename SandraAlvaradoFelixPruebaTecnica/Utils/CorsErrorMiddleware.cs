namespace SandraAlvaradoFelixPruebaTecnica.Utils
{
    public class CorsErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorsErrorMiddleware> _logger;

        public CorsErrorMiddleware(RequestDelegate next, ILogger<CorsErrorMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unhandled exception occurred.");

                // Handle the CORS error and return a JSON response
                context.Response.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                // You can customize the error message as needed
                var errorResponse = new
                {
                    Message = "Internal Server Error",
                    ExceptionMessage = ex.Message
                };

                await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(errorResponse));
            }
        }
    }
}
