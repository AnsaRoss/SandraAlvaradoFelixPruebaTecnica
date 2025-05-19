using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SandraAlvaradoFelixPruebaTecnica.Utils
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var result = new
                {
                    statusCode = 400,
                    message = string.Format("{0} - error : {1}", "One or more validation errors occurred", JsonConvert.SerializeObject(errors))

                };

                context.Result = new UnprocessableEntityObjectResult(result);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}