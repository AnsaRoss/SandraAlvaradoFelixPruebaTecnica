using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandraAlvaradoFelixPruebaTecnica.Models.Deportista
{
    public class Deportista
    {
        public class CrearDeportista
        {
            [JsonIgnore]
            [BindNever]
            public int user_id_i { get; set; }
            [Required(ErrorMessage = "El campo nombre es obligatorio.")]
            public string nombre { get; set; }
            [Required(ErrorMessage = "El campo pais_id es obligatorio.")]
            public int pais_id { get; set; }
            [JsonIgnore]
            [BindNever]
            public string ip_client { get; set; }

        }
    }
}
