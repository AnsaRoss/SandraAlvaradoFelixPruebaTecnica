using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;

namespace SandraAlvaradoFelixPruebaTecnica.Utils
{
    public class Sql
    {
        private SqlConnection connection = null;
        public Sql(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }
        public class GlobalResponse
        {
            public int result { get; set; }
            public string error_nv { get; set; }
            public JObject json { get; set; }
        }
        public GlobalResponse ExecuteProcedureDynamic(string nameProcedure, Dictionary<string, string> parameters)
        {
            try
            {
                if (string.IsNullOrEmpty(nameProcedure) || parameters == null)
                    throw new ArgumentException("El nombre del procedimiento y los parámetros no pueden ser nulos o vacíos.");

                var global = new GlobalResponse();
                SqlCommand command = connection.CreateCommand();
                command.CommandTimeout = 600;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = nameProcedure;

                foreach (var item in parameters)
                {
                    command.Parameters.AddWithValue(item.Key, item.Value);

                }

                command.Parameters.Add("@json_out_nv", SqlDbType.VarChar, int.MaxValue).Direction = ParameterDirection.Output;
                connection.Open();
                command.ExecuteNonQuery();
                if (command.Parameters["@json_out_nv"].Value != DBNull.Value)
                {
                    string jsonString = command.Parameters["@json_out_nv"].Value.ToString();
                    if (!string.IsNullOrEmpty(jsonString))
                    {
                        global = JsonConvert.DeserializeObject<GlobalResponse>(jsonString);
                        global.json = JObject.Parse(jsonString);
                        global.json.Remove("result");
                        global.json.Remove("error_nv");
                        if (global.json["json_result_nv"] is JObject jsonResultNv)
                        {
                            jsonResultNv.Remove("json_perm_nv");
                            global.json["json_result_nv"] = jsonResultNv;
                        }
                    }
                }

                return global;
            }
            catch (Exception ex)
            {
                connection.Close();
                throw new Exception($"{ex.Message} {ex.InnerException?.Message} {ex.InnerException?.InnerException?.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
