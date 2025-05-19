// using System;
// using System.Collections.Generic;
// using SaveSystem.Data;

// namespace SaveSystem.Data
// {
//     [System.Serializable]
//     public class GameData
//     {
//         // Version des données (pour migrations futures)
//         public string dataVersion = "1.0.0";
        
//         // Temps de jeu total
//         public float totalPlayTime = 0f;
        
//         // Date de dernière sauvegarde
//         public long lastSaveTimestamp;
        
//         // Données spécifiques au joueur
//         public PlayerData playerData = new PlayerData();
        
//         // Niveau actuel et état du monde
//         public string currentLevel = "MainMenu";
//         public SerializableDictionary<string, bool> discoveredAreas = new SerializableDictionary<string, bool>();
//         public SerializableDictionary<string, bool> unlockedAchievements = new SerializableDictionary<string, bool>();
        
//         // État des objets dans le monde (par ID)
//         public SerializableDictionary<string, bool> worldObjectStates = new SerializableDictionary<string, bool>();
        
//         // État des PNJ
//         public SerializableDictionary<string, int> npcStates = new SerializableDictionary<string, int>();
        
//         // Variables globales du jeu (pour scripts, quêtes, etc.)
//         public SerializableDictionary<string, string> gameVariables = new SerializableDictionary<string, string>();
        
//         // Constructeur
//         public GameData()
//         {
//             lastSaveTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
//         }
        
//         // Méthodes utilitaires pour les variables globales (différents types)
//         public void SetVariable(string key, string value)
//         {
//             gameVariables[key] = value;
//         }
        
//         public void SetVariable(string key, int value)
//         {
//             gameVariables[key] = value.ToString();
//         }
        
//         public void SetVariable(string key, float value)
//         {
//             gameVariables[key] = value.ToString();
//         }
        
//         public void SetVariable(string key, bool value)
//         {
//             gameVariables[key] = value.ToString();
//         }
        
//         public string GetString(string key, string defaultValue = "")
//         {
//             var dict = gameVariables.ToDictionary();
//             if (dict.ContainsKey(key))
//                 return dict[key];
//             return defaultValue;
//         }
        
//         public int GetInt(string key, int defaultValue = 0)
//         {
//             var dict = gameVariables.ToDictionary();
//             if (dict.ContainsKey(key) && int.TryParse(dict[key], out int value))
//                 return value;
//             return defaultValue;
//         }
        
//         public float GetFloat(string key, float defaultValue = 0f)
//         {
//             var dict = gameVariables.ToDictionary();
//             if (dict.ContainsKey(key) && float.TryParse(dict[key], out float value))
//                 return value;
//             return defaultValue;
//         }
        
//         public bool GetBool(string key, bool defaultValue = false)
//         {
//             var dict = gameVariables.ToDictionary();
//             if (dict.ContainsKey(key) && bool.TryParse(dict[key], out bool value))
//                 return value;
//             return defaultValue;
//         }
        
//         // Met à jour le timestamp
//         public void UpdateTimestamp()
//         {
//             lastSaveTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
//         }
//     }
// }


using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem.Data
{
    [Serializable]
    public class GameData
    {
        // Version des données (pour la compatibilité des sauvegardes)
        public int dataVersion = 1;
        
        // Horodatage de la création et dernière modification
        public long creationTimestamp;
        public long lastSaveTimestamp;
        
        // Données de niveau et joueur
        public string currentLevel = "";
        public float totalPlayTime = 0f;
        public PlayerData playerData = new PlayerData();
        
        // Collection de tous les états des entités sauvegardables
        public SerializableDictionary<string, Dictionary<string, object>> entityData = 
            new SerializableDictionary<string, Dictionary<string, object>>();
        
        // Variables globales utilisables pour la logique de jeu
        public SerializableDictionary<string, object> globalVariables = new SerializableDictionary<string, object>();
        
        // Constructeur
        public GameData()
        {
            // Horodatage de création
            creationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            lastSaveTimestamp = creationTimestamp;
        }
        
        // Met à jour l'horodatage de la dernière sauvegarde
        public void UpdateTimestamp()
        {
            lastSaveTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        
        // Définit une variable globale
        public void SetGlobalVariable(string key, object value)
        {
            globalVariables[key] = value;
        }
        
        // Récupère une variable globale typée
        public T GetGlobalVariable<T>(string key, T defaultValue = default)
        {
            if (globalVariables.ContainsKey(key))
            {
                try
                {
                    return (T)globalVariables[key];
                }
                catch (InvalidCastException)
                {
                    Debug.LogWarning($"Impossible de convertir la variable '{key}' en type {typeof(T).Name}");
                    return defaultValue;
                }
            }
            return defaultValue;
        }
        
        // Supprime une variable globale
        public bool RemoveGlobalVariable(string key)
        {
            return globalVariables.Remove(key);
        }
    }
}