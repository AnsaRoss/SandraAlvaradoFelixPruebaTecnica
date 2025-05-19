using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SandraAlvaradoFelixPruebaTecnica.Models.Login;
using SandraAlvaradoFelixPruebaTecnica.Utils;
using System.Net;
using System.Reflection;

namespace SandraAlvaradoFelixPruebaTecnica.Controllers
{
    [Route("api/login/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginController(IConfiguration configuration, ILogger<LoginController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("auth")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Login(Login login)
        {
            try
            {
                var request = _httpContextAccessor.HttpContext.Request;
                var conexion = "conexion";
                var sql = new Sql(_configuration.GetConnectionString(conexion));
                login.contrasena = Encript.EncryptPassword(login.contrasena);
                login.ip_client = HttpContext.Connection.RemoteIpAddress?.ToString();


                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("@json_in_nv", JsonConvert.SerializeObject(login));

                var response = sql.ExecuteProcedureDynamic(PathProcedure.procedureLogin, parameters);

                if (response.result != 1)
                {
                    LogHelper.RegistrarLog("Error en login",$"Fallo en procedimiento login: {response.error_nv}",0,
                        login.ip_client,PathProcedure.procedureLogin,null,new { error = response.error_nv }
                    );
                    return base.NotFound(ResponseMessage.Error(HttpStatusCode.NotFound, $"{response.error_nv}"));
                }
                response.json["json_result_nv"]["usr_name"] = login.usuario;
                response.json["json_result_nv"]["conexionName"] = conexion;
                var loginResponse = new
                {
                    jwt = Jwt.GenerateJwtToken(_configuration["TokenKey"], response.json),
                    user = response.json["json_result_nv"]
                };
                LogHelper.RegistrarLog("Login exitoso",$"Usuario autenticado correctamente: {login.usuario}",
                    (int)response.json["json_result_nv"]["id"],login.ip_client,PathProcedure.procedureLogin,null,
                    new { user = response.json["json_result_nv"] }
                );
                return Content(JsonConvert.SerializeObject(loginResponse), "application/json");
            }
            catch (Exception ex)
            {
                string message = $"Error en {MethodBase.GetCurrentMethod().Name} {ex.Message} {ex.InnerException?.Message} {ex.InnerException?.InnerException?.Message}";
                LogHelper.RegistrarLog("Error en sistema",message,0,HttpContext.Connection.RemoteIpAddress?.ToString(),
                    PathProcedure.procedureLogin,null,new { stack = ex.StackTrace }
                );
                return base.BadRequest(ResponseMessage.Error(HttpStatusCode.BadRequest, message));
            }
        }

    }
}