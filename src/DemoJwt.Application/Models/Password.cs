using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DemoJwt.Application.Models
{
    public sealed class Password
    {
        public string Encoded { get; }

        public Password(string password)
        {
            Encoded = EncodePassword(password);
        }

        public static implicit operator Password(string password) => new Password(password);

        private static string EncodePassword(string password)
        {
            string result;
            var data = Encoding.Unicode.GetBytes(password);
            byte[] bytes;

            using (var stream = new MemoryStream())
            {
                stream.WriteByte(0);

                // Converter os bytes em SHA512
                var sha512 = SHA512.Create();
                var hash = sha512.ComputeHash(data);
                stream.Write(hash, 0, hash.Length);
                bytes = stream.ToArray();
                result = Convert.ToBase64String(bytes);
                             
            }
            return result;
        }
    }
}