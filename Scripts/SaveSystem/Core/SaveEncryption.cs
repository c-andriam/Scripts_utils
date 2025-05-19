using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SaveSystem.Core
{
    public static class SaveEncryption
    {
        // Transforme une clé string en bytes pour AES
        public static byte[] GetKeyBytes(string key)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
        }
        
        // Crypte les données
        public static byte[] Encrypt(byte[] data, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = GetKeyBytes(key);
                aes.GenerateIV(); // IV unique pour chaque sauvegarde
                
                using (MemoryStream output = new MemoryStream())
                {
                    // Écrire l'IV dans le flux de sortie pour pouvoir décrypter plus tard
                    output.Write(aes.IV, 0, aes.IV.Length);
                    
                    using (CryptoStream cryptoStream = new CryptoStream(output, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                        return output.ToArray();
                    }
                }
            }
        }
        
        // Décrypte les données
        public static byte[] Decrypt(byte[] data, string key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = GetKeyBytes(key);
                
                // Lire l'IV depuis les données cryptées
                byte[] iv = new byte[aes.IV.Length];
                Array.Copy(data, 0, iv, 0, iv.Length);
                aes.IV = iv;
                
                using (MemoryStream output = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(
                        output, 
                        aes.CreateDecryptor(), 
                        CryptoStreamMode.Write))
                    {
                        // Décrypter les données sans l'IV
                        cryptoStream.Write(
                            data, 
                            aes.IV.Length, 
                            data.Length - aes.IV.Length);
                            
                        cryptoStream.FlushFinalBlock();
                        return output.ToArray();
                    }
                }
            }
        }
    }
}