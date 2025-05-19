namespace SandraAlvaradoFelixPruebaTecnica.Models.ModelResponses
{
    public class JsonResultNv
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string usuario { get; set; }
        public string rol { get; set; }
        public DateTime fecha_creacion { get; set; }
        public string correo { get; set; }
        public int estado { get; set; }
        public string conexionName { get; set; }
    }

    public class LoginResponseSql
    {
        public int result { get; set; }
        public int total_i { get; set; }
        public JsonResultNv json_result_nv { get; set; }
        public string error_nv { get; set; }

    }
}
