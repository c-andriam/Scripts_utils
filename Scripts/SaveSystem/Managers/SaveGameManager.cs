using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using SaveSystem.Core;
using SaveSystem.Data;

namespace SaveSystem.Managers
{
    public class SaveGameManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private int maxSaveSlots = 10;
        [SerializeField] private string autoSaveSlotName = "AutoSave";
        [SerializeField] private bool loadOnStart = true;
        [SerializeField] private float thumbnailWidth = 256f;
        [SerializeField] private float thumbnailHeight = 144f;
        
        [Header("Events")]
        public UnityEvent<bool, string> OnSaveCompleted;
        public UnityEvent<bool, string> OnLoadCompleted;
        public UnityEvent<string> OnBeforeSave;
        public UnityEvent<string> OnBeforeLoad;
        
        // États et données
        private GameData _currentGameData;
        private bool _isInitialized = false;
        private bool _isSaving = false;
        private bool _isLoading = false;
        
        // Propriétés publiques
        public GameData CurrentData => _currentGameData;
        public bool IsInitialized => _isInitialized;
        public bool IsSaving => _isSaving;
        public bool IsLoading => _isLoading;
        
        // Instance singleton
        public static SaveGameManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeSaveSystem();
        }
        
        private void Start()
        {
            if (loadOnStart)
            {
                // Charge la dernière sauvegarde ou l'autosave si disponible
                LoadLastSaveOrDefault();
            }
        }
        
        private void InitializeSaveSystem()
        {
            _currentGameData = new GameData();
            _isInitialized = true;
            Debug.Log("Système de sauvegarde initialisé");
        }
        
        // Sauvegarde le jeu dans un emplacement spécifique
        public void SaveGame(string slotName)
        {
            if (_isSaving || !_isInitialized) return;
            
            StartCoroutine(SaveGameRoutine(slotName));
        }
        
        private IEnumerator SaveGameRoutine(string slotName)
        {
            _isSaving = true;
            
            // Notification de pré-sauvegarde
            OnBeforeSave?.Invoke(slotName);
            
            // Attendre une frame pour permettre aux objets de se mettre à jour
            yield return null;
            
            // Capturer l'état actuel du jeu
            CaptureGameState();
            
            // Préparer les métadonnées
            SaveMetadata metadata = new SaveMetadata();
            metadata.saveVersion = _currentGameData.dataVersion;
            metadata.lastModified = DateTime.UtcNow;
            metadata.levelName = _currentGameData.currentLevel;
            metadata.playTime = _currentGameData.totalPlayTime;
            metadata.playerName = _currentGameData.playerData.playerName;
            
            // Capturer une miniature si nécessaire
            metadata.thumbnailBase64 = CaptureScreenshotAsBase64((int)thumbnailWidth, (int)thumbnailHeight);
            
            // Sauvegarder le jeu
            bool success = EnhancedBinarySaveSystem.SaveData(_currentGameData, slotName, metadata);
            string message = success ? "Sauvegarde réussie!" : "Échec de la sauvegarde!";
            
            _isSaving = false;
            
            // Notification de fin de sauvegarde
            OnSaveCompleted?.Invoke(success, message);
            
            Debug.Log($"Sauvegarde {slotName}: {message}");
        }
        
        // Charge le jeu depuis un emplacement spécifique
        public void LoadGame(string slotName)
        {
            if (_isLoading || !_isInitialized) return;
            
            StartCoroutine(LoadGameRoutine(slotName));
        }
        
        private IEnumerator LoadGameRoutine(string slotName)
        {
            _isLoading = true;
            
            // Notification de pré-chargement
            OnBeforeLoad?.Invoke(slotName);
            
            // Attendre une frame
            yield return null;
            
            // Charger le jeu
            var (data, metadata, success, error) = EnhancedBinarySaveSystem.LoadData<GameData>(slotName);
            
            if (success && data != null)
            {
                _currentGameData = data;
                ApplyGameState();
                Debug.Log($"Chargement de sauvegarde réussi: {slotName}");
            }
            else
            {
                Debug.LogWarning($"Échec du chargement de sauvegarde {slotName}: {error}");
            }
            
            string message = success ? "Chargement réussi!" : $"Échec du chargement: {error}";
            
            _isLoading = false;
            
            // Notification de fin de chargement
            OnLoadCompleted?.Invoke(success, message);
        }
        
        // Capture l'état actuel du jeu
        private void CaptureGameState()
        {
            if (_currentGameData == null)
            {
                _currentGameData = new GameData();
            }
            
            // Mettre à jour les données de base
            _currentGameData.UpdateTimestamp();
            _currentGameData.currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            
            // Trouver et capturer l'état du joueur
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _currentGameData.playerData.CaptureFromPlayer(player);
            }
            
            // Trouver et capturer l'état de tous les objets sauvegardables
            var saveableEntities = FindObjectsOfType<SaveableEntity>();
            foreach (var entity in saveableEntities)
            {
                entity.CaptureState(_currentGameData);
            }
        }
        
                // Applique l'état chargé au jeu
        private void ApplyGameState()
        {
            // Charger la scène si nécessaire
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (currentScene != _currentGameData.currentLevel)
            {
                // Si vous préférez changer de scène automatiquement, décommentez la ligne suivante
                // UnityEngine.SceneManagement.SceneManager.LoadScene(_currentGameData.currentLevel);
                
                // Note : il est généralement préférable de gérer le changement de scène séparément
                Debug.Log($"Note: La sauvegarde provient d'une scène différente ({_currentGameData.currentLevel})");
            }
            
            // Trouver et appliquer l'état au joueur
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _currentGameData.playerData.ApplyToPlayer(player);
            }
            else
            {
                Debug.LogWarning("Joueur non trouvé pour appliquer les données sauvegardées");
            }
            
            // Appliquer l'état à tous les objets sauvegardables
            var saveableEntities = FindObjectsOfType<SaveableEntity>();
            foreach (var entity in saveableEntities)
            {
                entity.ApplyState(_currentGameData);
            }
        }
        
        // Charge la dernière sauvegarde ou la sauvegarde par défaut
        private void LoadLastSaveOrDefault()
        {
            var saves = GetSavesList();
            
            // Essayer d'abord l'autosave
            if (EnhancedBinarySaveSystem.SaveExists(autoSaveSlotName))
            {
                LoadGame(autoSaveSlotName);
                return;
            }
            
            // Sinon, charger la sauvegarde la plus récente
            if (saves.Count > 0)
            {
                // Trier par date
                var mostRecent = saves.OrderByDescending(s => s.metadata.lastModified).FirstOrDefault();
                if (mostRecent != null)
                {
                    LoadGame(mostRecent.slotName);
                    return;
                }
            }
            
            // Si aucune sauvegarde n'existe, initialiser une nouvelle partie
            InitNewGame();
        }
        
        // Initialise une nouvelle partie
        public void InitNewGame()
        {
            _currentGameData = new GameData();
            Debug.Log("Nouvelle partie initialisée");
            
            // Vous pouvez définir des valeurs par défaut ici
            _currentGameData.playerData.playerName = "Player";
            _currentGameData.playerData.health = 100;
            _currentGameData.playerData.maxHealth = 100;
            
            // Notifier les écouteurs
            OnLoadCompleted?.Invoke(true, "Nouvelle partie initialisée");
        }
        
        // Supprime une sauvegarde
        public void DeleteSave(string slotName)
        {
            bool success = EnhancedBinarySaveSystem.DeleteSave(slotName);
            Debug.Log(success ? $"Sauvegarde {slotName} supprimée" : $"Échec de la suppression {slotName}");
        }
        
        // Obtient la liste de toutes les sauvegardes
        public List<SaveInfo> GetSavesList()
        {
            List<SaveInfo> saveInfos = new List<SaveInfo>();
            
            var slots = EnhancedBinarySaveSystem.GetAllSaveSlots();
            foreach (string slotName in slots)
            {
                var (metadata, success) = EnhancedBinarySaveSystem.GetSaveMetadata(slotName);
                if (success && metadata != null)
                {
                    saveInfos.Add(new SaveInfo
                    {
                        slotName = slotName,
                        metadata = metadata
                    });
                }
            }
            
            return saveInfos;
        }
        
        // Capture une capture d'écran pour la miniature
        private string CaptureScreenshotAsBase64(int width, int height)
        {
            // Désactiver temporairement l'UI si nécessaire
            // var canvases = FindObjectsOfType<Canvas>().Where(c => c.gameObject.activeSelf).ToList();
            // foreach (var canvas in canvases) canvas.gameObject.SetActive(false);
            
            try
            {
                // Créer une texture de rendu
                RenderTexture rt = new RenderTexture(width, height, 24);
                Camera.main.targetTexture = rt;
                
                // Prendre une capture d'écran
                Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
                Camera.main.Render();
                RenderTexture.active = rt;
                screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                
                // Réinitialiser la caméra
                Camera.main.targetTexture = null;
                RenderTexture.active = null;
                Destroy(rt);
                
                // Convertir en Base64
                byte[] bytes = screenshot.EncodeToPNG();
                string base64 = Convert.ToBase64String(bytes);
                
                Destroy(screenshot);
                return base64;
            }
            catch (Exception e)
            {
                Debug.LogError($"Erreur lors de la capture d'écran: {e.Message}");
                return string.Empty;
            }
            finally
            {
                // Réactiver l'UI
                // foreach (var canvas in canvases) canvas.gameObject.SetActive(true);
            }
        }
        
        // Vérifie si une sauvegarde est disponible
        public bool HasSave(string slotName)
        {
            return EnhancedBinarySaveSystem.SaveExists(slotName);
        }
        
        // Structure pour stocker les informations de sauvegarde
        public class SaveInfo
        {
            public string slotName;
            public SaveMetadata metadata;
        }
    }
}
