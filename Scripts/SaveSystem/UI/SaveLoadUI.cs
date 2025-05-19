using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SaveSystem.Managers;
using SaveSystem.Core;

namespace SaveSystem.UI
{
    public class SaveLoadUI : MonoBehaviour
    {
        [Header("Références")]
        [SerializeField] private GameObject saveSlotPrefab;
        [SerializeField] private Transform saveSlotContainer;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button backButton;
        [SerializeField] private GameObject loadingPanel;
        [SerializeField] private TextMeshProUGUI statusText;
        
        [Header("Configuration")]
        [SerializeField] private int maxSlots = 10;
        [SerializeField] private string newSavePrefix = "Save";
        
        private SaveGameManager saveManager;
        private List<SaveSlotUI> slots = new List<SaveSlotUI>();
        private bool isSaveMode = true;
        
        private void Start()
        {
            saveManager = SaveGameManager.Instance;
            
            if (saveManager == null)
            {
                Debug.LogError("SaveGameManager non trouvé");
                gameObject.SetActive(false);
                return;
            }
            
            // Ajouter des écouteurs d'événements
            saveManager.OnSaveCompleted.AddListener(OnSaveCompleted);
            saveManager.OnLoadCompleted.AddListener(OnLoadCompleted);
            
            // Configurer les boutons
            if (newGameButton != null)
                newGameButton.onClick.AddListener(OnNewGameClicked);
            
            if (backButton != null)
                backButton.onClick.AddListener(OnBackClicked);
            
            // Cacher le panneau de chargement
            if (loadingPanel != null)
                loadingPanel.SetActive(false);
            
            // Initialiser l'UI
            RefreshSaveSlots();
        }
        
        private void OnDestroy()
        {
            if (saveManager != null)
            {
                saveManager.OnSaveCompleted.RemoveListener(OnSaveCompleted);
                saveManager.OnLoadCompleted.RemoveListener(OnLoadCompleted);
            }
        }
        
        public void SetSaveMode(bool isSaveMode)
        {
            this.isSaveMode = isSaveMode;
            RefreshSaveSlots();
            
            // Gérer le bouton nouvelle partie (uniquement visible en mode chargement)
            if (newGameButton != null)
                newGameButton.gameObject.SetActive(!isSaveMode);
        }
        
        public void RefreshSaveSlots()
        {
            // Nettoyer les anciens slots
            foreach (Transform child in saveSlotContainer)
            {
                Destroy(child.gameObject);
            }
            slots.Clear();
            
            // Obtenir toutes les sauvegardes
            var saves = saveManager.GetSavesList();
            
            // Trier par date
            saves = saves.OrderByDescending(s => s.metadata.lastModified).ToList();
            
            // Créer des slots pour les sauvegardes existantes
            foreach (var save in saves)
            {
                CreateSlotForSave(save);
            }
            
            // En mode sauvegarde, ajouter un slot vide si nécessaire
            if (isSaveMode && slots.Count < maxSlots)
            {
                CreateEmptySlot();
            }
        }
        
        private void CreateSlotForSave(SaveGameManager.SaveInfo saveInfo)
        {
            if (slots.Count >= maxSlots) return;
            
            GameObject slotGO = Instantiate(saveSlotPrefab, saveSlotContainer);
            SaveSlotUI slot = slotGO.GetComponent<SaveSlotUI>();
            
            if (slot != null)
            {
                slot.Initialize(
                    saveInfo.slotName,
                    saveInfo.metadata.playerName,
                    FormatDateTime(saveInfo.metadata.lastModified),
                    saveInfo.metadata.levelName,
                    FormatPlayTime(saveInfo.metadata.playTime),
                    saveInfo.metadata.thumbnailBase64
                );
                
                // Configurer les actions
                slot.OnSlotClicked += () => OnSlotClicked(saveInfo.slotName);
                slot.OnDeleteClicked += () => OnDeleteClicked(saveInfo.slotName);
                
                // En mode chargement, ne pas permettre la suppression si autosave
                if (!isSaveMode && saveInfo.slotName == "AutoSave")
                    slot.SetDeleteButtonVisible(false);
                
                slots.Add(slot);
            }
        }
        
        private void CreateEmptySlot()
        {
            GameObject slotGO = Instantiate(saveSlotPrefab, saveSlotContainer);
            SaveSlotUI slot = slotGO.GetComponent<SaveSlotUI>();
            
            if (slot != null)
            {
                string newSlotName = GenerateNewSlotName();
                
                slot.Initialize(
                    newSlotName,
                    "Nouvel emplacement",
                    "Vide",
                    "",
                    "",
                    ""
                );
                
                slot.SetEmpty(true);
                slot.OnSlotClicked += () => OnSlotClicked(newSlotName);
                slot.OnDeleteClicked += () => { }; // Pas de suppression pour les nouveaux emplacements
                slot.SetDeleteButtonVisible(false);
                
                slots.Add(slot);
            }
        }
        
        private void OnSlotClicked(string slotName)
        {
            if (isSaveMode)
            {
                // Mode sauvegarde
                ShowLoadingPanel("Sauvegarde en cours...");
                saveManager.SaveGame(slotName);
            }
            else
            {
                // Mode chargement
                if (saveManager.HasSave(slotName))
                {
                    ShowLoadingPanel("Chargement en cours...");
                    saveManager.LoadGame(slotName);
                }
            }
        }
        
        private void OnDeleteClicked(string slotName)
        {
            // Afficher une boîte de dialogue de confirmation ici si souhaité
            saveManager.DeleteSave(slotName);
            RefreshSaveSlots();
        }
        
        private void OnNewGameClicked()
        {
            // Ici, vous pouvez soit démarrer directement une nouvelle partie
            // soit rediriger vers un écran de création de personnage
            ShowLoadingPanel("Initialisation d'une nouvelle partie...");
            saveManager.InitNewGame();
            
            // Fermer le panneau de sauvegarde/chargement
            gameObject.SetActive(false);
        }
        
        private void OnBackClicked()
        {
            // Fermer le panneau
            gameObject.SetActive(false);
        }
        
        private void ShowLoadingPanel(string message)
        {
            if (loadingPanel != null)
            {
                loadingPanel.SetActive(true);
                
                if (statusText != null)
                    statusText.text = message;
            }
        }
        
        private void HideLoadingPanel()
        {
            if (loadingPanel != null)
                loadingPanel.SetActive(false);
        }
        
        private void OnSaveCompleted(bool success, string message)
        {
            HideLoadingPanel();
            
            // Afficher un message de succès/échec
            Debug.Log(message);
            
            // Rafraîchir la liste
            RefreshSaveSlots();
        }
        
        private void OnLoadCompleted(bool success, string message)
        {
            HideLoadingPanel();
            
            if (success)
            {
                // Fermer le panneau de sauvegarde/chargement
                gameObject.SetActive(false);
            }
            else
            {
                // Afficher un message d'erreur
                Debug.LogWarning(message);
            }
        }
        
        private string GenerateNewSlotName()
        {
            int index = 1;
            string slotName;
            
            do
            {
                slotName = $"{newSavePrefix}{index}";
                index++;
            }
            while (saveManager.HasSave(slotName) && index <= maxSlots);
            
            return slotName;
        }
        
        private string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy HH:mm");
        }
        
        private string FormatPlayTime(float playTimeInSeconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(playTimeInSeconds);
            return $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }
    }
}