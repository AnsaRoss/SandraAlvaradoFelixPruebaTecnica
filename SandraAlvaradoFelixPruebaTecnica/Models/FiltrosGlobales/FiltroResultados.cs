using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace SandraAlvaradoFelixPruebaTecnica.Models.FiltrosGlobales
{
    public class FiltroResultados
    {
        [JsonIgnore]
        [BindNever]
        public int user_id_i { get; set; }
        public int? skip_i { get; set; }
        public int? take_i { get; set; }
        [JsonIgnore]
        [BindNever]
        public string ip_client { get; set; }
    }
}
