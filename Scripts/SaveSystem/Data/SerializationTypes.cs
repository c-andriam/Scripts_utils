using System;

namespace SaveSystem.Core
{
    // Interface pour les objets qui peuvent être sauvegardés
    public interface ISerializable { }
    
    // Extension pour permettre que les contraintes génériques fonctionnent
    public static class SerializableExtensions
    {
        public static bool IsSerializable<T>(this T obj) where T : class
        {
            return obj is ISerializable || obj.GetType().IsSerializable;
        }
    }
}

// Ensuite, modifiez les contraintes génériques dans tous vos fichiers
// De: where T : Serializable
// À:  where T : class