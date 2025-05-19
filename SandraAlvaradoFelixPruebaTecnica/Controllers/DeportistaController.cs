using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SandraAlvaradoFelixPruebaTecnica.Utils;
using System.Net;
using System.Reflection;
using static SandraAlvaradoFelixPruebaTecnica.Models.Deportista.Deportista;
using SandraAlvaradoFelixPruebaTecnica.Models.ModelResponses;

namespace SandraAlvaradoFelixPruebaTecnica.Controllers
{
    [Route("api/deportista/")]
    [ApiController]
    public class DeportistaController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DeportistaController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeportistaController(IConfiguration configuration, ILogger<DeportistaController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("crear")]
        [Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult CrearDeportista(CrearDeportista crearDeportista)
        {
            try
            {
                var request = _httpContextAccessor.HttpContext.Request;
                Request.Headers.TryGetValue("Authorization", out StringValues headerValue);
                var token = Jwt.ReadJwtToken(headerValue, _configuration["TokenKey"], new LoginResponseSql());
                if(token == null)
                {
                    LogHelper.RegistrarLog( "Token inválido", "Token inválido recibido", 0, HttpContext.Connection.RemoteIpAddress?.ToString(),
                        PathProcedure.procedureCrearDeportista, null, null
                    );
                    return base.BadRequest(ResponseMessage.Error(HttpStatusCode.BadRequest, PathMessage.MessageTokenInvalid));
                }
                crearDeportista.user_id_i = token.json_result_nv.id;
                crearDeportista.ip_client = HttpContext.Connection.RemoteIpAddress?.ToString();

                LogHelper.RegistrarLog( "Inicio de proceso", "Inicio de la creación del deportista", crearDeportista.user_id_i,
                    crearDeportista.ip_client, PathProcedure.procedureCrearDeportista, JsonConvert.SerializeObject(crearDeportista), null
                );
                var sql = new Sql(_configuration.GetConnectionString(token.json_result_nv.conexionName));


                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("@json_in_nv", JsonConvert.SerializeObject(crearDeportista));

                var response = sql.ExecuteProcedureDynamic(PathProcedure.procedureCrearDeportista, parameters);

                if (response.result != 1)
                {
                    LogHelper.RegistrarLog( "Error en ejecución", "Error al ejecutar el procedimiento para crear el deportista",
                        crearDeportista.user_id_i, crearDeportista.ip_client, PathProcedure.procedureCrearDeportista,
                        null, new { error = response.error_nv }
                    );
                    return base.NotFound(ResponseMessage.Error(HttpStatusCode.NotFound, $"{response.error_nv}"));
                }
                if (response.json["json_result_nv"]["code"].ToString() == "0")
                {
                    LogHelper.RegistrarLog( "Error en proceso", "Error en el proceso de creación del deportista", crearDeportista.user_id_i,
                        crearDeportista.ip_client, PathProcedure.procedureCrearDeportista, JsonConvert.SerializeObject(crearDeportista), response.json["json_result_nv"]
                    );
                    return base.NotFound(ResponseMessage.Error(HttpStatusCode.NotFound, $"{PathMessage.MessageErrorProcess}"));
                }
                LogHelper.RegistrarLog( "Deportista creado exitosamente", "Deportista creado con éxito", crearDeportista.user_id_i,
                        crearDeportista.ip_client, PathProcedure.procedureCrearDeportista, JsonConvert.SerializeObject(crearDeportista), response.json["json_result_nv"].ToString()
                    );
                return base.Ok(ResponseMessage.Ok());
            }
            catch (Exception ex)
            {
                string message = $"Error en {MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.InnerException?.Message} {ex.InnerException?.InnerException?.Message}";
                _logger.LogError(message);
                return base.BadRequest(ResponseMessage.Error(HttpStatusCode.BadRequest, message));
            }
        }
    }
}
