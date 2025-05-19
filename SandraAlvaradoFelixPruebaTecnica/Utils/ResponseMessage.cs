using System.Net;

namespace SandraAlvaradoFelixPruebaTecnica.Utils
{
    public class ResponseMessage
    {
        public static object Error(HttpStatusCode statusCode, string error)
        {
            return new { statusCode = statusCode, message = error };
        }

        public static object Ok()
        {
            return new { message = "Transacción exitosa." };
        }
    }
}
