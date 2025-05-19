using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SandraAlvaradoFelixPruebaTecnica.Models.FiltrosGlobales;
using SandraAlvaradoFelixPruebaTecnica.Models.ModelResponses;
using SandraAlvaradoFelixPruebaTecnica.Utils;
using Serilog;
using System.Net;
using System.Reflection;
using static SandraAlvaradoFelixPruebaTecnica.Models.Levantamiento.Levantamiento;

namespace SandraAlvaradoFelixPruebaTecnica.Controllers
{
    [Route("api/levantamiento/")]
    [ApiController]
    public class LevantamientoController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LevantamientoController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LevantamientoController(IConfiguration configuration, ILogger<LevantamientoController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost("crear")]
        [Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult CrearLevantamiento(CrearLevantamiento crearLevantamiento)
        {
            try
            {
                var modalidadesValidas = new[] { "arranque", "envion" };
                var modalidad = crearLevantamiento.modalidad?.Trim().ToLower();
                if (!modalidadesValidas.Contains(modalidad))
                {
                    LogHelper.RegistrarLog( "Modalidad inválida", "Modalidad inválida recibida", 0,HttpContext.Connection.RemoteIpAddress?.ToString(),
                        PathProcedure.procedureCrearLevantamiento, null, new { error = "Modalidad inválida" }
                    );
                    return BadRequest(new
                    {
                        mensaje = "La modalidad debe ser 'aranque' o 'envion'."
                    });
                }
                crearLevantamiento.modalidad = modalidad;

                var request = _httpContextAccessor.HttpContext.Request;
                Request.Headers.TryGetValue("Authorization", out StringValues headerValue);
                var token = Jwt.ReadJwtToken(headerValue, _configuration["TokenKey"], new LoginResponseSql());
                if (token == null)
                {
                    LogHelper.RegistrarLog( "Token inválido","Token inválido recibido",0,HttpContext.Connection.RemoteIpAddress?.ToString(),
                        PathProcedure.procedureCrearLevantamiento,null,null
                    );
                    //_logger.LogWarning("Token inválido recibido para el usuario con IP {Ip}",
                    return base.BadRequest(ResponseMessage.Error(HttpStatusCode.BadRequest, PathMessage.MessageTokenInvalid));
                }
                crearLevantamiento.user_id_i = token.json_result_nv.id;
                crearLevantamiento.ip_client = HttpContext.Connection.RemoteIpAddress?.ToString();

                LogHelper.RegistrarLog( "Inicio de proceso","Inicio de la creación del levantamiento",crearLevantamiento.user_id_i,
                    crearLevantamiento.ip_client,PathProcedure.procedureCrearLevantamiento, JsonConvert.SerializeObject(crearLevantamiento), null
                );
                var sql = new Sql(_configuration.GetConnectionString(token.json_result_nv.conexionName));


                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("@json_in_nv", JsonConvert.SerializeObject(crearLevantamiento));

                var response = sql.ExecuteProcedureDynamic(PathProcedure.procedureCrearLevantamiento, parameters);

                if (response.result != 1)
                {
                    LogHelper.RegistrarLog( "Error en ejecución","Error al ejecutar el procedimiento para crear el levantamiento",
                        crearLevantamiento.user_id_i,crearLevantamiento.ip_client,PathProcedure.procedureCrearLevantamiento,
                        JsonConvert.SerializeObject(crearLevantamiento), new { error = response.error_nv }
                    );
                    return base.NotFound(ResponseMessage.Error(HttpStatusCode.NotFound, $"{response.error_nv}"));
                }
                if (response.json["json_result_nv"]["code"].ToString() == "0")
                {
                    LogHelper.RegistrarLog( "Error en proceso","Error en el proceso de creación del levantamiento",crearLevantamiento.user_id_i,
                        crearLevantamiento.ip_client,PathProcedure.procedureCrearLevantamiento, JsonConvert.SerializeObject(crearLevantamiento), response.json["json_result_nv"]
                    );
                    return base.NotFound(ResponseMessage.Error(HttpStatusCode.NotFound, $"{PathMessage.MessageErrorProcess}"));
                }
                LogHelper.RegistrarLog( "Levantamiento creado exitosamente","Levantamiento creado con éxito",crearLevantamiento.user_id_i,
                        crearLevantamiento.ip_client,PathProcedure.procedureCrearLevantamiento, JsonConvert.SerializeObject(crearLevantamiento), response.json["json_result_nv"].ToString()
                    );
                return base.Ok(ResponseMessage.Ok());
            }
            catch (Exception ex)
            {
                string message = $"Error en {MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.InnerException?.Message} {ex.InnerException?.InnerException?.Message}";
                LogHelper.RegistrarLog( "Excepción no controlada",message,0, HttpContext.Connection.RemoteIpAddress?.ToString(),
                    PathProcedure.procedureCrearLevantamiento,null,new { stack = ex.StackTrace }
                );
                return base.BadRequest(ResponseMessage.Error(HttpStatusCode.BadRequest, message));
            }
        }
        [HttpGet("intentos")]
        [Authorize]
        public IActionResult ListarIntentos([FromQuery] FiltroResultados filter)
        {
            try
            {
                var request = _httpContextAccessor.HttpContext.Request;
                Request.Headers.TryGetValue("Authorization", out StringValues headerValue);
                var token = Jwt.ReadJwtToken(headerValue, _configuration["TokenKey"], new LoginResponseSql());
                if (token == null)
                {
                    LogHelper.RegistrarLog( "Token inválido","Token inválido recibido",0,HttpContext.Connection.RemoteIpAddress?.ToString(),
                        PathProcedure.procedureListarIntentos, null,null
                    );
                    return base.BadRequest(ResponseMessage.Error(HttpStatusCode.BadRequest, PathMessage.MessageTokenInvalid));
                }
                filter.user_id_i = token.json_result_nv.id;
                filter.ip_client = HttpContext.Connection.RemoteIpAddress?.ToString();

                LogHelper.RegistrarLog( "Inicio de proceso","Inicio de la obtención de listar intentos levantamientos",filter.user_id_i,
                    filter.ip_client,PathProcedure.procedureListarIntentos, JsonConvert.SerializeObject(filter), null
                );
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("@json_in_nv", JsonConvert.SerializeObject(filter));

                var sql = new Sql(_configuration.GetConnectionString(token.json_result_nv.conexionName));

                var response = sql.ExecuteProcedureDynamic(PathProcedure.procedureListarIntentos, parameters);

                if (response.result != 1)
                {
                    LogHelper.RegistrarLog( "Error en ejecución","Error al ejecutar el procedimiento listar intentos de levantamientos",
                        filter.user_id_i,filter.ip_client,PathProcedure.procedureListarIntentos, JsonConvert.SerializeObject(filter), new { error = response.error_nv }
                    );
                    return base.NotFound(ResponseMessage.Error(HttpStatusCode.NotFound, $"{response.error_nv}"));
                }

                LogHelper.RegistrarLog( "Lista de levantamientos obtenidos", "Lista de levantamientos obtenidos con éxito", filter.user_id_i, 
                    filter.ip_client,PathProcedure.procedureListarIntentos, JsonConvert.SerializeObject(filter), response.json["json_result_nv"].ToString()
                );

                return Content(response.json["json_result_nv"].ToString(), "application/json");
            }
            catch (Exception ex)
            {
                string message = $"Error en {MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.InnerException?.Message} {ex.InnerException?.InnerException?.Message}";
                LogHelper.RegistrarLog( "Excepción no controlada",message,0,HttpContext.Connection.RemoteIpAddress?.ToString(),
                    PathProcedure.procedureListarIntentos, null,new { stack = ex.StackTrace }
                );
                return base.BadRequest(ResponseMessage.Error(HttpStatusCode.BadRequest, message));
            }
        }
    }
}
