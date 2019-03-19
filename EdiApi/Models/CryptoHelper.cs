using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EdiApi.Models
{
    public static class CryptoHelper
    {
        public static byte[] EncryptData(byte[] data)
        {
            byte[] key = Encoding.UTF8.GetBytes("5113B615B2FFA181108FA843A09F0775");
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            csEncrypt.Write(data, 0, data.Length);

                        return msEncrypt.ToArray();
                    }
                }
            }

        }
        public static byte[] DecryptData(byte[] encrypted)
        {
            byte[] key = Encoding.UTF8.GetBytes("5113B615B2FFA181108FA843A09F0775");
            var iv = new byte[16];
            Buffer.BlockCopy(encrypted, 0, iv, 0, iv.Length);
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    using (var msDecrypt = new MemoryStream(encrypted, iv.Length, encrypted.Length - iv.Length))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var resultStream = new MemoryStream())
                            {
                                csDecrypt.CopyTo(resultStream);
                                return resultStream.ToArray();
                            }
                        }
                    }
                }
            }
        }
    }
}
