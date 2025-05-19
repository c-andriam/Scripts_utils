// using System;
// using UnityEngine;

// namespace SaveSystem.Core
// {
//     [System.Serializable]
//     public class SaveMetadata
//     {
//         public string saveVersion;        // Version du format de sauvegarde
//         public string gameVersion;        // Version du jeu
//         public DateTime creationDate;     // Date de création
//         public DateTime lastModified;     // Dernière modification
//         public string playerName;         // Nom du joueur (si applicable)
//         public float playTime;            // Temps de jeu
//         public string levelName;          // Niveau actuel
//         public string checksum;           // Checksum pour vérification
//         public string thumbnailBase64;    // Capture d'écran encodée en Base64 (optionnel)
        
//         public SaveMetadata()
//         {
//             saveVersion = "1.0.0";
//             gameVersion = Application.version;
//             creationDate = DateTime.UtcNow;
//             lastModified = creationDate;
//             playTime = 0f;
//         }
//     }
// }


using System;
using UnityEngine;

namespace SaveSystem.Core
{
    [Serializable]
    public class SaveMetadata
    {
        // Changer le type de string à int pour être compatible avec GameData.dataVersion
        public int saveVersion;
        public DateTime lastModified;
        public string levelName;
        public float playTime;
        public string playerName;
        public string thumbnailBase64;
        public string checksum;  // Pour vérifier l'intégrité des données
        
        public SaveMetadata()
        {
            lastModified = DateTime.UtcNow;
            saveVersion = 1;  // Version par défaut
        }
    }
}