// using System;
// using System.Collections.Generic;

// namespace SaveSystem.Data
// {
//     [Serializable]
//     public class SerializableDictionary<TKey, TValue>
//     {
//         [Serializable]
//         public class KeyValuePairData
//         {
//             public TKey Key;
//             public TValue Value;
            
//             public KeyValuePairData(TKey key, TValue value)
//             {
//                 Key = key;
//                 Value = value;
//             }
//         }
        
//         public List<KeyValuePairData> Pairs = new List<KeyValuePairData>();
        
//         public SerializableDictionary() { }
        
//         public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
//         {
//             FromDictionary(dictionary);
//         }
        
//         public void FromDictionary(Dictionary<TKey, TValue> dictionary)
//         {
//             Pairs.Clear();
            
//             if (dictionary == null)
//                 return;
                
//             foreach (var kvp in dictionary)
//             {
//                 Pairs.Add(new KeyValuePairData(kvp.Key, kvp.Value));
//             }
//         }
        
//         public Dictionary<TKey, TValue> ToDictionary()
//         {
//             Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            
//             foreach (var pair in Pairs)
//             {
//                 if (pair.Key != null && !dict.ContainsKey(pair.Key))
//                 {
//                     dict.Add(pair.Key, pair.Value);
//                 }
//             }
            
//             return dict;
//         }
        
//         // Ajout d'indexation pour usage plus simple
//         public TValue this[TKey key]
//         {
//             get
//             {
//                 foreach (var pair in Pairs)
//                 {
//                     if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
//                     {
//                         return pair.Value;
//                     }
//                 }
//                 throw new KeyNotFoundException($"La clé {key} n'existe pas dans le dictionnaire.");
//             }
//             set
//             {
//                 bool found = false;
//                 for (int i = 0; i < Pairs.Count; i++)
//                 {
//                     if (EqualityComparer<TKey>.Default.Equals(Pairs[i].Key, key))
//                     {
//                         Pairs[i].Value = value;
//                         found = true;
//                         break;
//                     }
//                 }
                
//                 if (!found)
//                 {
//                     Pairs.Add(new KeyValuePairData(key, value));
//                 }
//             }
//         }
//     }
// }

using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem.Data
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        // Ces listes seront sérialisées
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();
        
        // Dictionnaire en mémoire pour l'accès rapide
        private Dictionary<TKey, TValue> _dict;
        
        // Accesseur au dictionnaire interne
        private Dictionary<TKey, TValue> Dict
        {
            get
            {
                if (_dict == null)
                {
                    _dict = new Dictionary<TKey, TValue>();
                    for (int i = 0; i < Math.Min(keys.Count, values.Count); i++)
                    {
                        if (keys[i] != null && !_dict.ContainsKey(keys[i]))
                        {
                            _dict.Add(keys[i], values[i]);
                        }
                    }
                }
                return _dict;
            }
        }
        
        // Accesseur par clé (indexeur)
        public TValue this[TKey key]
        {
            get
            {
                return Dict[key];
            }
            set
            {
                if (Dict.ContainsKey(key))
                {
                    Dict[key] = value;
                    int index = keys.IndexOf(key);
                    if (index >= 0)
                    {
                        values[index] = value;
                    }
                }
                else
                {
                    Dict.Add(key, value);
                    keys.Add(key);
                    values.Add(value);
                }
            }
        }
        
        // Vérifie si une clé existe
        public bool ContainsKey(TKey key)
        {
            return Dict.ContainsKey(key);
        }
        
        // Supprime une entrée
        public bool Remove(TKey key)
        {
            if (Dict.ContainsKey(key))
            {
                int index = keys.IndexOf(key);
                if (index >= 0)
                {
                    keys.RemoveAt(index);
                    values.RemoveAt(index);
                }
                return Dict.Remove(key);
            }
            return false;
        }
        
        // Vide le dictionnaire
        public void Clear()
        {
            Dict.Clear();
            keys.Clear();
            values.Clear();
        }
        
        // Importe depuis un dictionnaire standard
        public void FromDictionary(Dictionary<TKey, TValue> dict)
        {
            Clear();
            foreach (var kvp in dict)
            {
                this[kvp.Key] = kvp.Value;
            }
        }
        
        // Exporte vers un dictionnaire standard
        public Dictionary<TKey, TValue> ToDictionary()
        {
            return new Dictionary<TKey, TValue>(Dict);
        }
        
        // Retourne le nombre d'éléments
        public int Count => Dict.Count;
        
        // Accès aux clés et valeurs
        public Dictionary<TKey, TValue>.KeyCollection Keys => Dict.Keys;
        public Dictionary<TKey, TValue>.ValueCollection Values => Dict.Values;
    }
}