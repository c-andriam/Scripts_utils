using System;
using System.Security.Cryptography;

namespace SaveSystem.Core
{
    public static class SaveIntegrityChecker
    {
        // Calcule un checksum SHA-256 pour vérifier l'intégrité
        public static string CalculateChecksum(byte[] data)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(data);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
        
        // Vérifie l'intégrité des données en comparant avec le checksum stocké
        public static bool VerifyIntegrity(byte[] data, string storedChecksum)
        {
            string calculatedChecksum = CalculateChecksum(data);
            return calculatedChecksum == storedChecksum;
        }
    }
}