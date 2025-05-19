using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SandraAlvaradoFelixPruebaTecnica.Models.FiltrosGlobales;
using SandraAlvaradoFelixPruebaTecnica.Models.ModelResponses;
using SandraAlvaradoFelixPruebaTecnica.Utils;
using System.Net;
using System.Reflection;

namespace SandraAlvaradoFelixPruebaTecnica.Controllers
{
    [Route("api/resultados/")]
    [ApiController]

    public class ResultadosController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ResultadosController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResultadosController(IConfiguration configuration, ILogger<ResultadosController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Authorize]
        public IActionResult ListarResultados([FromQuery] FiltroResultados filter)
        {
            try
            {
                var request = _httpContextAccessor.HttpContext.Request;
                Request.Headers.TryGetValue("Authorization", out StringValues headerValue);
                //valid token
                var token = Jwt.ReadJwtToken(headerValue, _configuration["TokenKey"], new LoginResponseSql());
                if (token == null)
                {
                    LogHelper.RegistrarLog("Token inválido", "Token inválido recibido", 0, HttpContext.Connection.RemoteIpAddress?.ToString(),
                        PathProcedure.procedureListarResultados, null, null
                    );
                    return base.BadRequest(ResponseMessage.Error(HttpStatusCode.BadRequest, PathMessage.MessageTokenInvalid));
                }
                filter.user_id_i = token.json_result_nv.id;
                filter.ip_client = HttpContext.Connection.RemoteIpAddress?.ToString();

                LogHelper.RegistrarLog("Inicio de proceso", "Inicio de obtención de resultados", filter.user_id_i,
                     filter.ip_client, PathProcedure.procedureListarResultados, JsonConvert.SerializeObject(filter), null
                 );

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("@json_in_nv", JsonConvert.SerializeObject(filter));

                var sql = new Sql(_configuration.GetConnectionString(token.json_result_nv.conexionName));

                var response = sql.ExecuteProcedureDynamic(PathProcedure.procedureListarResultados, parameters);

                if (response.result != 1)
                {
                    LogHelper.RegistrarLog("Error en ejecución", "Error al ejecutar el procedimiento listar resultados",
                        filter.user_id_i, filter.ip_client, PathProcedure.procedureListarResultados, null, new { error = response.error_nv }
                    );
                    return base.NotFound(ResponseMessage.Error(HttpStatusCode.NotFound, $"{response.error_nv}"));
                }

                LogHelper.RegistrarLog("Lista de resultados", "Lista de resultados con éxito", filter.user_id_i,
                    filter.ip_client, PathProcedure.procedureListarResultados, JsonConvert.SerializeObject(filter), response.json["json_result_nv"].ToString()
                );

                return Content(response.json["json_result_nv"].ToString(), "application/json");
            }
            catch (Exception ex)
            {
                string message = $"Error en {MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.InnerException?.Message} {ex.InnerException?.InnerException?.Message}";
                LogHelper.RegistrarLog("Excepción no controlada", message, 0, HttpContext.Connection.RemoteIpAddress?.ToString(),
                    PathProcedure.procedureListarResultados, null, new { stack = ex.StackTrace }
                );
                return base.BadRequest(ResponseMessage.Error(HttpStatusCode.BadRequest, message));
            }
        }

    }
}
