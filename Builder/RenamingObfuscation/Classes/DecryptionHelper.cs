using System.Collections.Generic;

namespace Server.RenamingObfuscation.Classes
{
    internal static class DecryptionHelper
    {
        public static string Update(string t)
        {
            string k = "[KEY]";
            List<byte> resStr = new List<byte>();
            int i = 0;
            foreach (var c in t)
            {
                resStr.Add((byte)(c ^ k[i++]));
                i = i % k.Length;
            }
            t = System.Text.Encoding.Default.GetString(resStr.ToArray());
            return t;
        }
    }
}
