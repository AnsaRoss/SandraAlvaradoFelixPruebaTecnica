using Newtonsoft.Json;
using Serilog;

namespace SandraAlvaradoFelixPruebaTecnica.Utils
{
    public static class LogHelper
    {
        public static void RegistrarLog(
        string evento,
        string eventoDescripcion,
        int usuarioId,
        string ipCliente,
        string nombreProcedimiento,
        object jsonInRequest,
        object jsonOutResponse)
        {
            // Convierte los objetos a JSON para que puedan ser guardados
            var jsonIn = System.Text.Json.JsonSerializer.Serialize(jsonInRequest);
            var jsonOut = System.Text.Json.JsonSerializer.Serialize(jsonOutResponse);

            // Usamos Serilog para registrar el log
            Log.ForContext("evento", evento)
               .ForContext("evento_descripcion", eventoDescripcion)
               .ForContext("usuario", usuarioId)
               .ForContext("cliente_ip", ipCliente)
               .ForContext("nombre_procedimiento", nombreProcedimiento)
               .ForContext("json_in_nv_request", jsonIn)
               .ForContext("json_out_nv_response", jsonOut)
               .Information($"Log de Auditoría Registrado - Evento: {evento}, Usuario: {usuarioId}, Procedimiento: {nombreProcedimiento}"); 
        }
    }
}
