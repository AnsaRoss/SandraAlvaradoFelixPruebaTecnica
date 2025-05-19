using System.Security.Cryptography;
using System.Text;

namespace SandraAlvaradoFelixPruebaTecnica.Utils
{
    public class Encript
    {
        public static string EncryptPassword(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Convertir el array de bytes a una cadena hexadecimal
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    //Indica que el valor hexadecimal debe tener al menos dos dígitos
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
