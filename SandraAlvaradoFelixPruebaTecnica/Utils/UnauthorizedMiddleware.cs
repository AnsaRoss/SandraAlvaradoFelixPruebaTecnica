using Newtonsoft.Json;

namespace SandraAlvaradoFelixPruebaTecnica.Utils
{
    public class UnauthorizedMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 401 && !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var error = new
                {
                    statusCode = context.Response.StatusCode,
                    message = "Unauthorized"
                };

                var json = JsonConvert.SerializeObject(error);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
