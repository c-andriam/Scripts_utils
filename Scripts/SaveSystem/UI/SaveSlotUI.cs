using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SaveSystem.UI
{
    public class SaveSlotUI : MonoBehaviour
    {
        [Header("Références")]
        [SerializeField] private Button slotButton;
        [SerializeField] private Button deleteButton;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private TextMeshProUGUI dateText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private RawImage thumbnailImage;
        
        [Header("Styles")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color emptyColor = new Color(0.8f, 0.8f, 0.8f);
        
        public event Action OnSlotClicked;
        public event Action OnDeleteClicked;
        
        private string slotId;
        private bool isEmpty = false;
        
        private void Awake()
        {
            if (slotButton != null)
                slotButton.onClick.AddListener(() => OnSlotClicked?.Invoke());
            
            if (deleteButton != null)
                deleteButton.onClick.AddListener(() => OnDeleteClicked?.Invoke());
        }
        
        public void Initialize(string id, string title, string date, string level, string time, string thumbnailBase64)
        {
            slotId = id;
            
            if (titleText != null)
                titleText.text = title;
            
            if (subtitleText != null)
                subtitleText.text = id;
            
            if (dateText != null)
                dateText.text = date;
            
            if (levelText != null)
                levelText.text = level;
            
            if (timeText != null)
                timeText.text = time;
            
            // Charger la miniature si disponible
            if (thumbnailImage != null && !string.IsNullOrEmpty(thumbnailBase64))
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(thumbnailBase64);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(bytes);
                    thumbnailImage.texture = texture;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Erreur lors du chargement de la miniature: {e.Message}");
                }
            }
        }
        
        public void SetEmpty(bool isEmpty)
        {
            this.isEmpty = isEmpty;
            
            // Appliquer un style différent si vide
            if (slotButton != null)
            {
                ColorBlock colors = slotButton.colors;
                colors.normalColor = isEmpty ? emptyColor : normalColor;
                slotButton.colors = colors;
            }
            
            // Cacher le bouton supprimer
            if (deleteButton != null)
                deleteButton.gameObject.SetActive(!isEmpty);
        }
        
        public void SetDeleteButtonVisible(bool isVisible)
        {
            if (deleteButton != null)
                deleteButton.gameObject.SetActive(isVisible);
        }
        
        public string GetSlotId()
        {
            return slotId;
        }
    }
}