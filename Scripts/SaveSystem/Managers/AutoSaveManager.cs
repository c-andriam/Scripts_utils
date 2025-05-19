using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem.Managers
{
    public class AutoSaveManager : MonoBehaviour
    {
        [Header("Auto-save Configuration")]
        [Tooltip("Intervalle entre les sauvegardes automatiques en minutes")]
        public float autoSaveInterval = 5f;
        
        [Tooltip("Activer la sauvegarde automatique")]
        public bool enableAutoSave = true;
        
        [Tooltip("Sauvegarde automatique lors du changement de scène")]
        public bool saveOnSceneChange = true;
        
        [Tooltip("Sauvegarde automatique lors de la fermeture du jeu")]
        public bool saveOnQuit = true;
        
        [Tooltip("Sauvegarde automatique lors de la mise en pause (mobile)")]
        public bool saveOnPause = true;
        
        private float timeSinceLastSave = 0f;
        private SaveGameManager saveManager;
        
        private void Start()
        {
            saveManager = GetComponent<SaveGameManager>();
            
            if (saveManager == null)
            {
                saveManager = FindObjectOfType<SaveGameManager>();
                
                if (saveManager == null)
                {
                    Debug.LogError("Aucun SaveGameManager trouvé pour l'AutoSaveManager");
                    enabled = false;
                    return;
                }
            }
            
            // S'abonner aux événements de changement de scène
            if (saveOnSceneChange)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
        }
        
        private void OnDestroy()
        {
            if (saveOnSceneChange)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }
        
        private void Update()
        {
            if (!enableAutoSave) return;
            
            timeSinceLastSave += Time.deltaTime;
            
            if (timeSinceLastSave >= autoSaveInterval * 60f) // Conversion en secondes
            {
                PerformAutoSave();
                timeSinceLastSave = 0f;
            }
        }
        
        private void OnApplicationQuit()
        {
            if (saveOnQuit)
            {
                PerformAutoSave();
            }
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if (saveOnPause && pauseStatus) // Jeu passé en arrière-plan
            {
                PerformAutoSave();
            }
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (saveOnSceneChange)
            {
                // Attendre un peu pour que les objets de la scène soient initialisés
                StartCoroutine(SaveAfterDelay(0.5f));
            }
        }
        
        private IEnumerator SaveAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            PerformAutoSave();
        }
        
        public void PerformAutoSave()
        {
            if (saveManager != null && saveManager.IsInitialized)
            {
                saveManager.SaveGame("AutoSave");
                Debug.Log("Sauvegarde automatique effectuée");
            }
        }
    }
}