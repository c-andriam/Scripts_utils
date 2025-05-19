using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace SaveSystem.Core
{
    public static class EnhancedBinarySaveSystem
    {
        // Clé de cryptage - À changer pour chaque projet
        private static string EncryptionKey = "Change_This_Key_For_Each_Project_2025";
        
        // Répertoire de base pour les sauvegardes
        private static string SaveDirectory => Path.Combine(Application.persistentDataPath, "Saves");
        
        // Initialise le système
        static EnhancedBinarySaveSystem()
        {
            // Créer le répertoire s'il n'existe pas
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }
        
        // Retourne le chemin complet pour un fichier de sauvegarde
        public static string GetSavePath(string saveSlotName, string fileExtension = ".sav")
        {
            return Path.Combine(SaveDirectory, $"{saveSlotName}{fileExtension}");
        }
        
        // Sauvegarde des données avec une sauvegarde de secours
        public static bool SaveData<T>(T data, string saveSlotName, SaveMetadata metadata = null) where T : class
        {
            string path = GetSavePath(saveSlotName);
            string tempPath = path + ".tmp";
            string backupPath = path + ".bak";
            
            try
            {
                // Créer les métadonnées si non fournies
                if (metadata == null)
                {
                    metadata = new SaveMetadata();
                }
                else
                {
                    metadata.lastModified = DateTime.UtcNow;
                }
                
                // Sérialiser les données
                byte[] serializedData;
                using (MemoryStream memStream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(memStream, data);
                    serializedData = memStream.ToArray();
                }
                
                // Ajouter le checksum aux métadonnées
                metadata.checksum = SaveIntegrityChecker.CalculateChecksum(serializedData);
                
                // Sauvegarder dans un fichier temporaire
                using (FileStream stream = new FileStream(tempPath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    
                    // Écrire les métadonnées
                    formatter.Serialize(stream, metadata);
                    
                    // Écrire les données cryptées
                    byte[] encryptedData = SaveEncryption.Encrypt(serializedData, EncryptionKey);
                    stream.Write(encryptedData, 0, encryptedData.Length);
                }
                
                // Effectuer une sauvegarde de l'ancien fichier si existant
                if (File.Exists(path))
                {
                    if (File.Exists(backupPath))
                        File.Delete(backupPath);
                    
                    File.Move(path, backupPath);
                }
                
                // Renommer le fichier temporaire
                File.Move(tempPath, path);
                
                Debug.Log($"Sauvegarde réussie dans: {path}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Erreur lors de la sauvegarde: {e.Message}");
                
                // Nettoyer les fichiers temporaires
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
                    
                // Restaurer depuis la sauvegarde si besoin
                if (!File.Exists(path) && File.Exists(backupPath))
                {
                    File.Copy(backupPath, path);
                    Debug.Log("Restauration à partir de la sauvegarde");
                }
                
                return false;
            }
        }
        
        // Charge les données avec gestion des erreurs et sauvegarde de secours
        public static (T data, SaveMetadata metadata, bool success, string error) LoadData<T>(string saveSlotName) where T : class
        {
            string path = GetSavePath(saveSlotName);
            string backupPath = path + ".bak";
            
            // Essayer d'abord le fichier principal
            var primaryResult = TryLoadFile<T>(path);
            if (primaryResult != null && primaryResult.Value.success)
            {
                return primaryResult.Value;
            }
            
            // En cas d'échec, essayer la sauvegarde
            var backupResult = TryLoadFile<T>(backupPath);
            if (backupResult != null && backupResult.Value.success)
            {
                Debug.Log("Chargement depuis la sauvegarde de secours");
                
                // Restaurer la sauvegarde principale depuis la sauvegarde
                try { File.Copy(backupPath, path, true); } 
                catch { Debug.LogWarning("Impossible de restaurer la sauvegarde principale"); }
                
                return backupResult.Value;
            }
            
            // Les deux ont échoué
            return (default(T), null, false, 
                "Impossible de charger la sauvegarde et sa sauvegarde de secours");
        }
        
        // Essaie de charger un fichier unique avec gestion des erreurs
        private static (T data, SaveMetadata metadata, bool success, string error)? TryLoadFile<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath))
                return null;
            
            try
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                
                using (MemoryStream memStream = new MemoryStream(fileData))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    
                    // Lire les métadonnées
                    SaveMetadata metadata = (SaveMetadata)formatter.Deserialize(memStream);
                    
                    // Lire les données cryptées (le reste du fichier)
                    byte[] encryptedData = new byte[fileData.Length - memStream.Position];
                    memStream.Read(encryptedData, 0, encryptedData.Length);
                    
                    // Décrypter les données
                    byte[] decryptedData;
                    try
                    {
                        decryptedData = SaveEncryption.Decrypt(encryptedData, EncryptionKey);
                    }
                    catch (Exception e)
                    {
                        return (default(T), metadata, false, $"Erreur de décryptage: {e.Message}");
                    }
                    
                    // Vérifier l'intégrité des données
                    string checksum = SaveIntegrityChecker.CalculateChecksum(decryptedData);
                    if (checksum != metadata.checksum)
                    {
                        return (default(T), metadata, false, 
                            "Corruption détectée: le checksum ne correspond pas");
                    }
                    
                    // Désérialiser les données
                    using (MemoryStream dataStream = new MemoryStream(decryptedData))
                    {
                        try
                        {
                            T data = (T)formatter.Deserialize(dataStream);
                            return (data, metadata, true, null);
                        }
                        catch (Exception e)
                        {
                            return (default(T), metadata, false, 
                                $"Erreur de désérialisation: {e.Message}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return (default(T), null, false, $"Erreur de chargement: {e.Message}");
            }
        }
        
        // Vérifie si une sauvegarde existe
        public static bool SaveExists(string saveSlotName)
        {
            string path = GetSavePath(saveSlotName);
            return File.Exists(path);
        }
        
        // Supprime une sauvegarde
        public static bool DeleteSave(string saveSlotName)
        {
            try
            {
                string path = GetSavePath(saveSlotName);
                string backupPath = path + ".bak";
                
                if (File.Exists(path))
                    File.Delete(path);
                    
                if (File.Exists(backupPath))
                    File.Delete(backupPath);
                    
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Erreur lors de la suppression: {e.Message}");
                return false;
            }
        }
        
        // Obtient les métadonnées d'une sauvegarde sans charger toutes les données
        public static (SaveMetadata metadata, bool success) GetSaveMetadata(string saveSlotName)
        {
            string path = GetSavePath(saveSlotName);
            
            if (!File.Exists(path))
                return (null, false);
                
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    SaveMetadata metadata = (SaveMetadata)formatter.Deserialize(stream);
                    return (metadata, true);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Erreur de lecture des métadonnées: {e.Message}");
                return (null, false);
            }
        }
        
        // Obtient la liste de toutes les sauvegardes disponibles
        public static List<string> GetAllSaveSlots()
        {
            List<string> saveSlots = new List<string>();
            
            try
            {
                if (!Directory.Exists(SaveDirectory))
                    return saveSlots;
                    
                string[] files = Directory.GetFiles(SaveDirectory, "*.sav");
                
                foreach (string file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    saveSlots.Add(fileName);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Erreur lors de la recherche des sauvegardes: {e.Message}");
            }
            
            return saveSlots;
        }
    }
}