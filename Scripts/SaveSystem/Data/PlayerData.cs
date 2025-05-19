// using System;
// using UnityEngine;
// using SaveSystem.Data;

// namespace SaveSystem.Data
// {
//     [Serializable]
//     public class PlayerData
//     {
//         // Données de base du joueur
//         public string playerName = "Player";
//         public float health = 100f;
//         public float maxHealth = 100f;
//         public int level = 1;
//         public float experience = 0f;
//         public SerializableVector3 position;
//         public SerializableVector3 rotation;
        
//         // Stats de progression
//         public int killCount = 0;
//         public int deathCount = 0;
//         public float playTime = 0f;
        
//         // Inventaire (facilement extensible)
//         public SerializableDictionary<string, int> inventory = new SerializableDictionary<string, int>();
        
//         // Compétences débloquées
//         public bool[] unlockedSkills = new bool[20]; // Pré-allocation pour 20 skills
        
//         // Quêtes
//         public SerializableDictionary<string, int> questProgress = new SerializableDictionary<string, int>();
        
//         // Captures les données du joueur depuis les composants
//         public void CaptureFromPlayer(GameObject player)
//         {
//             if (player == null) return;
            
//             // Position et rotation
//             position = player.transform.position;
//             rotation = new SerializableVector3(player.transform.eulerAngles);
            
//             // Autres composants (ajuster en fonction de votre jeu)
//             var healthComponent = player.GetComponent<PlayerHealth>(); // Exemple
//             if (healthComponent != null)
//             {
//                 health = healthComponent.CurrentHealth;
//                 maxHealth = healthComponent.MaxHealth;
//             }
            
//             var statsComponent = player.GetComponent<PlayerStats>(); // Exemple
//             if (statsComponent != null)
//             {
//                 level = statsComponent.Level;
//                 experience = statsComponent.Experience;
//             }
            
//             var inventoryComponent = player.GetComponent<PlayerInventory>(); // Exemple
//             if (inventoryComponent != null)
//             {
//                 inventory.FromDictionary(inventoryComponent.GetSerializableInventory());
//             }
//         }
        
//         // Applique les données au joueur
//         public void ApplyToPlayer(GameObject player)
//         {
//             if (player == null) return;
            
//             // Position et rotation
//             player.transform.position = position;
//             player.transform.eulerAngles = rotation;
            
//             // Autres composants (ajuster en fonction de votre jeu)
//             var healthComponent = player.GetComponent<PlayerHealth>(); // Exemple
//             if (healthComponent != null)
//             {
//                 healthComponent.SetHealth(health);
//                 healthComponent.SetMaxHealth(maxHealth);
//             }
            
//             var statsComponent = player.GetComponent<PlayerStats>(); // Exemple
//             if (statsComponent != null)
//             {
//                 statsComponent.SetLevel(level);
//                 statsComponent.SetExperience(experience);
//             }
            
//             var inventoryComponent = player.GetComponent<PlayerInventory>(); // Exemple
//             if (inventoryComponent != null)
//             {
//                 inventoryComponent.LoadInventory(inventory.ToDictionary());
//             }
//         }
//     }
// }

using System;
using UnityEngine;
using SaveSystem.Data;

namespace SaveSystem.Data
{
    [Serializable]
    public class PlayerData
    {
        // Données de base du joueur
        public string playerName = "Player";
        public float health = 100f;
        public float maxHealth = 100f;
        public int level = 1;
        public float experience = 0f;
        public SerializableVector3 position;
        public SerializableVector3 rotation;
        
        // Stats de progression
        public int killCount = 0;
        public int deathCount = 0;
        public float playTime = 0f;
        
        // Inventaire (facilement extensible)
        public SerializableDictionary<string, int> inventory = new SerializableDictionary<string, int>();
        
        // Compétences débloquées
        public bool[] unlockedSkills = new bool[20]; // Pré-allocation pour 20 skills
        
        // Quêtes
        public SerializableDictionary<string, int> questProgress = new SerializableDictionary<string, int>();
        
        // Captures les données du joueur depuis les composants
        public void CaptureFromPlayer(GameObject player)
        {
            if (player == null) return;
            
            // Position et rotation
            position = player.transform.position;
            rotation = new SerializableVector3(player.transform.eulerAngles);
            
            // Commenté jusqu'à ce que vous implémentiez ces classes
            /*
            // Autres composants (ajuster en fonction de votre jeu)
            var healthComponent = player.GetComponent<PlayerHealth>();
            if (healthComponent != null)
            {
                health = healthComponent.CurrentHealth;
                maxHealth = healthComponent.MaxHealth;
            }
            
            var statsComponent = player.GetComponent<PlayerStats>();
            if (statsComponent != null)
            {
                level = statsComponent.Level;
                experience = statsComponent.Experience;
            }
            
            var inventoryComponent = player.GetComponent<PlayerInventory>();
            if (inventoryComponent != null)
            {
                inventory.FromDictionary(inventoryComponent.GetSerializableInventory());
            }
            */
        }
        
        // Applique les données au joueur
        public void ApplyToPlayer(GameObject player)
        {
            if (player == null) return;
            
            // Position et rotation
            player.transform.position = position;
            player.transform.eulerAngles = rotation;
            
            // Commenté jusqu'à ce que vous implémentiez ces classes
            /*
            // Autres composants (ajuster en fonction de votre jeu)
            var healthComponent = player.GetComponent<PlayerHealth>();
            if (healthComponent != null)
            {
                healthComponent.SetHealth(health);
                healthComponent.SetMaxHealth(maxHealth);
            }
            
            var statsComponent = player.GetComponent<PlayerStats>();
            if (statsComponent != null)
            {
                statsComponent.SetLevel(level);
                statsComponent.SetExperience(experience);
            }
            
            var inventoryComponent = player.GetComponent<PlayerInventory>();
            if (inventoryComponent != null)
            {
                inventoryComponent.LoadInventory(inventory.ToDictionary());
            }
            */
        }
    }
}