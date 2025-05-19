using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SandraAlvaradoFelixPruebaTecnica.Models.Levantamiento
{
    public class Levantamiento
    {
        public class CrearLevantamiento
        {
            [JsonIgnore]
            [BindNever]
            public int user_id_i { get; set; }
            [Required(ErrorMessage = "El campo deportista_id es obligatorio.")]
            public int deportista_id { get; set; }
            [Required(ErrorMessage = "El campo modalidad es obligatorio.")]
            public string modalidad { get; set; }
            [Required(ErrorMessage = "El campo peso es obligatorio.")]
            public int peso { get; set; }
            [JsonIgnore]
            [BindNever]
            public string ip_client { get; set; }

        }
    }
}
