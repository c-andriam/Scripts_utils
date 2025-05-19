using System;
using System.Collections.Generic;

namespace SaveSystem.Data
{
    /// <summary>
    /// Classe qui permet de sérialiser une liste dans Unity
    /// Cette classe est nécessaire car Unity ne peut pas sérialiser directement les types génériques List<T>
    /// </summary>
    [Serializable]
    public class SerializableList<T>
    {
        // La liste interne qui stocke les éléments
        public List<T> Items = new List<T>();
        
        // Constructeur par défaut
        public SerializableList() { }
        
        // Constructeur à partir d'une liste existante
        public SerializableList(List<T> list)
        {
            FromList(list);
        }
        
        // Conversion d'une liste standard vers SerializableList
        public void FromList(List<T> list)
        {
            Items.Clear();
            
            if (list == null)
                return;
                
            foreach (var item in list)
            {
                Items.Add(item);
            }
        }
        
        // Conversion de SerializableList vers une liste standard
        public List<T> ToList()
        {
            return new List<T>(Items);
        }
        
        // Propriété qui retourne le nombre d'éléments
        public int Count => Items.Count;
        
        // Indexeur pour accéder aux éléments comme une liste normale
        public T this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }
        
        // Méthodes utilitaires pour manipuler la liste
        
        public void Add(T item)
        {
            Items.Add(item);
        }
        
        public bool Remove(T item)
        {
            return Items.Remove(item);
        }
        
        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }
        
        public void Clear()
        {
            Items.Clear();
        }
        
        public bool Contains(T item)
        {
            return Items.Contains(item);
        }
        
        public int IndexOf(T item)
        {
            return Items.IndexOf(item);
        }
        
        public void Insert(int index, T item)
        {
            Items.Insert(index, item);
        }
        
        public T[] ToArray()
        {
            return Items.ToArray();
        }
        
        public void AddRange(IEnumerable<T> collection)
        {
            Items.AddRange(collection);
        }
        
        public void Sort()
        {
            Items.Sort();
        }
        
        public void Sort(Comparison<T> comparison)
        {
            Items.Sort(comparison);
        }
        
        public void Sort(IComparer<T> comparer)
        {
            Items.Sort(comparer);
        }
        
        // Support d'énumération pour utiliser foreach
        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}