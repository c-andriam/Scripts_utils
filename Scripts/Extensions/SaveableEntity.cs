// using UnityEngine;
// using SaveSystem.Data;
// using System.Collections.Generic;

// namespace SaveSystem.Extensions
// {
//     /// <summary>
//     /// Attacher ce composant à n'importe quel GameObject dont l'état doit être sauvegardé
//     /// </summary>
//     public class SaveableEntity : MonoBehaviour
//     {
//         [SerializeField] private string uniqueId = System.Guid.NewGuid().ToString();
        
//         [Tooltip("Si vrai, un nouvel ID sera généré à chaque fois que le préfab est placé")]
//         [SerializeField] private bool generateNewIdOnStart = false;
        
//         private static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();
        
//         private void Awake()
//         {
//             if (generateNewIdOnStart)
//             {
//                 uniqueId = System.Guid.NewGuid().ToString();
//             }
//         }
        
//         private void OnEnable()
//         {
//             // Enregistrer cet objet dans le lookup global
//             if (string.IsNullOrEmpty(uniqueId))
//             {
//                 uniqueId = System.Guid.NewGuid().ToString();
//                 Debug.LogWarning($"SaveableEntity sur {gameObject.name} n'avait pas d'ID. Nouvel ID généré: {uniqueId}");
//             }
            
//             if (globalLookup.ContainsKey(uniqueId))
//             {
//                 Debug.LogError($"ID en double trouvé: {uniqueId} sur {gameObject.name}. Cet ID est déjà utilisé par {globalLookup[uniqueId].gameObject.name}");
//                 uniqueId = System.Guid.NewGuid().ToString();
//                 Debug.Log($"Un nouvel ID a été généré: {uniqueId}");
//             }
            
//             globalLookup[uniqueId] = this;
//         }
        
//         private void OnDisable()
//         {
//             // Supprimer du lookup global
//             if (globalLookup.ContainsKey(uniqueId))
//             {
//                 globalLookup.Remove(uniqueId);
//             }
//         }
        
//         // Capturer l'état de tous les ISaveableComponent sur cet objet
//         public void CaptureState(GameData gameData)
//         {
//             // Vérifier si l'objet est actif/existant
//             gameData.worldObjectStates[uniqueId] = gameObject.activeSelf;
            
//             // Capturer la position/rotation
//             var positionData = new SerializableVector3(transform.position);
//             var rotationData = new SerializableVector3(transform.eulerAngles);
            
//             gameData.SetVariable($"{uniqueId}_pos_x", positionData.x);
//             gameData.SetVariable($"{uniqueId}_pos_y", positionData.y);
//             gameData.SetVariable($"{uniqueId}_pos_z", positionData.z);
//             gameData.SetVariable($"{uniqueId}_rot_x", rotationData.x);
//             gameData.SetVariable($"{uniqueId}_rot_y", rotationData.y);
//             gameData.SetVariable($"{uniqueId}_rot_z", rotationData.z);
            
//             // Chercher tous les composants sauvegardables
//             foreach (var saveable in GetComponents<ISaveable>())
//             {
//                 saveable.CaptureState(gameData, uniqueId);
//             }
//         }
        
//         // Appliquer l'état sauvegardé à tous les ISaveableComponent sur cet objet
//         public void ApplyState(GameData gameData)
//         {
//             // Vérifier si l'objet a un état sauvegardé
//             var dict = gameData.worldObjectStates.ToDictionary();
//             if (dict.ContainsKey(uniqueId))
//             {
//                 gameObject.SetActive(dict[uniqueId]);
//             }
            
//             // Appliquer la position/rotation
//             float x = gameData.GetFloat($"{uniqueId}_pos_x", transform.position.x);
//             float y = gameData.GetFloat($"{uniqueId}_pos_y", transform.position.y);
//             float z = gameData.GetFloat($"{uniqueId}_pos_z", transform.position.z);
//             transform.position = new Vector3(x, y, z);
            
//             float rotX = gameData.GetFloat($"{uniqueId}_rot_x", transform.eulerAngles.x);
//             float rotY = gameData.GetFloat($"{uniqueId}_rot_y", transform.eulerAngles.y);
//             float rotZ = gameData.GetFloat($"{uniqueId}_rot_z", transform.eulerAngles.z);
//             transform.eulerAngles = new Vector3(rotX, rotY, rotZ);
            
//             // Appliquer l'état aux composants
//             foreach (var saveable in GetComponents<ISaveable>())
//             {
//                 saveable.ApplyState(gameData, uniqueId);
//             }
//         }
        
//         // Obtenir l'ID unique
//         public string GetUniqueId()
//         {
//             return uniqueId;
//         }
        
//         // Générer un nouvel ID
//         [ContextMenu("Générer un nouvel ID")]
//         private void GenerateNewId()
//         {
//             uniqueId = System.Guid.NewGuid().ToString();
//         }
        
//         // Trouver une entité par son ID
//         public static SaveableEntity FindById(string id)
//         {
//             if (globalLookup.ContainsKey(id))
//             {
//                 return globalLookup[id];
//             }
//             return null;
//         }
//     }
    
//     // Interface pour les composants sauvegardables
//     public interface ISaveable
//     {
//         void CaptureState(GameData gameData, string objectId);
//         void ApplyState(GameData gameData, string objectId);
//     }
// }

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystem.Data;

namespace SaveSystem.Managers
{
    /// <summary>
    /// Composant à attacher à tout objet du jeu qui doit être sauvegardé
    /// </summary>
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueID = System.Guid.NewGuid().ToString();
        [Tooltip("Si vrai, cet objet sera sauvegardé même s'il est détruit")]
        [SerializeField] private bool persistWhenDestroyed = false;
        
        private static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        // Composants qui implémentent l'interface ISaveable
        private ISaveable[] saveableComponents;
        
        private void Awake()
        {
            saveableComponents = GetComponentsInChildren<ISaveable>();
        }
        
        private void OnEnable()
        {
            // Enregistrement de l'entité dans le dictionnaire global
            if (string.IsNullOrEmpty(uniqueID))
            {
                uniqueID = System.Guid.NewGuid().ToString();
            }
            
            if (globalLookup.ContainsKey(uniqueID))
            {
                Debug.LogWarning($"ID d'entité en double détecté: {uniqueID}. Cela peut causer des problèmes de sauvegarde.", this);
                uniqueID = System.Guid.NewGuid().ToString();
            }
            
            globalLookup[uniqueID] = this;
        }
        
        private void OnDisable()
        {
            // Désenregistrement de l'entité sauf si elle doit persister
            if (!persistWhenDestroyed)
            {
                globalLookup.Remove(uniqueID);
            }
        }
        
        // Génère un nouvel ID unique pour l'entité
        public void GenerateNewID()
        {
            if (Application.isPlaying)
            {
                globalLookup.Remove(uniqueID);
                uniqueID = System.Guid.NewGuid().ToString();
                globalLookup[uniqueID] = this;
            }
            else
            {
                uniqueID = System.Guid.NewGuid().ToString();
            }
        }
        
        // Capture l'état de cette entité et l'ajoute aux données du jeu
        public void CaptureState(GameData gameData)
        {
            if (saveableComponents == null || saveableComponents.Length == 0)
            {
                return;
            }
            
            // Créer un dictionnaire pour stocker les états de tous les composants sauvegardables
            Dictionary<string, object> state = new Dictionary<string, object>();
            
            foreach (var saveable in saveableComponents)
            {
                object componentState = saveable.CaptureState();
                if (componentState != null)
                {
                    // Utiliser le nom du type comme clé pour le composant
                    string typeName = saveable.GetType().ToString();
                    state[typeName] = componentState;
                }
            }
            
            // Ne sauvegarder que si nous avons des données à sauvegarder
            if (state.Count > 0)
            {
                gameData.entityData[uniqueID] = state;
            }
        }
        
        // Applique l'état sauvegardé à cette entité
        public void ApplyState(GameData gameData)
        {
            if (saveableComponents == null || saveableComponents.Length == 0)
            {
                return;
            }
            
            // Vérifier si nous avons des données pour cette entité
            if (!gameData.entityData.ContainsKey(uniqueID))
            {
                return;
            }
            
            Dictionary<string, object> state = gameData.entityData[uniqueID];
            
            foreach (var saveable in saveableComponents)
            {
                string typeName = saveable.GetType().ToString();
                
                if (state.ContainsKey(typeName))
                {
                    saveable.RestoreState(state[typeName]);
                }
            }
        }
        
        // Restaure une entité détruite ou crée une nouvelle instance
        public static SaveableEntity RestoreEntity(string id, GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (globalLookup.ContainsKey(id))
            {
                return globalLookup[id];
            }
            
            if (prefab == null)
            {
                Debug.LogWarning($"Prefab manquant pour l'entité: {id}");
                return null;
            }
            
            GameObject instance = Instantiate(prefab, position, rotation);
            SaveableEntity entity = instance.GetComponent<SaveableEntity>();
            
            if (entity == null)
            {
                Debug.LogWarning("Le prefab ne contient pas de composant SaveableEntity");
                return null;
            }
            
            entity.uniqueID = id;
            globalLookup[id] = entity;
            
            return entity;
        }
    }
    
    // Interface pour les composants sauvegardables
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}