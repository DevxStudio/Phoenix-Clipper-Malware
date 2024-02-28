using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Server.RenamingObfuscation.Classes
{
    public static class Utils
    {
        public static string GenerateRandomString()
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= random.Next(10,20); i++)
            {
                var randomCharacterPosition = random.Next(0, alphabet.Length);
                sb.Append(alphabet[randomCharacterPosition]);
            }
            return sb.ToString();
        }

        private static readonly Random random = new Random();
        static string alphabet = @"q%w%e%r%t%y%u%i%o%p%a%s%d%f%g%h%j%k%l%z%x%c%v%b%n%m%Q%W%E%R%T%Y%U%I%O%P%A%S%D%F%G%H%J%K%L%Z%X%C%V%B%N%M%".Replace("%", "");
    }
}
