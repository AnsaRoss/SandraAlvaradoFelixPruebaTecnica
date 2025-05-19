using System.Text.Json.Serialization;

namespace SandraAlvaradoFelixPruebaTecnica.Models.Login
{
    public class Login
    {
        public string usuario { get; set; }
        public string contrasena { get; set; }
        [JsonIgnore]
        public string jwt { get; set; }
        [JsonIgnore]
        public string ip_client { get; set; }
    }
}
