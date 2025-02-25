using System.Security.Cryptography;
using System.Text;

namespace SisAt.Helper
{
    public static class Criptografia
    {
        public static string GerarHash(this string valor)
        {
            var hash = SHA1.Create();
            var encode = new ASCIIEncoding();
            var array = encode.GetBytes(valor);

            array = hash.ComputeHash(array);

            var stringHexa = new StringBuilder();

            foreach (var item in array)
            {
                stringHexa.Append(item.ToString("x2"));
            }

            return stringHexa.ToString();
        }
    }
}
