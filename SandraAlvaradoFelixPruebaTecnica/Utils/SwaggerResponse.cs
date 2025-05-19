using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SandraAlvaradoFelixPruebaTecnica.Utils
{
    public class SwaggerResponse : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey("200") && context.MethodInfo.ReturnType != typeof(void))
            {
                var responseType = context.MethodInfo.ReturnType;
                var responseMediaType = operation.Responses["200"].Content.Keys.FirstOrDefault();

                if (responseMediaType != null)
                {
                    operation.Responses["200"].Content[responseMediaType].Example = GetExampleForType(responseType);
                }
            }
        }

        private IOpenApiAny GetExampleForType(Type type)
        {
            return new OpenApiString(JsonConvert.SerializeObject(type));
        }

        public class LoginSwagger
        {
            public string jwt { get; set; }
        }
    }
}
