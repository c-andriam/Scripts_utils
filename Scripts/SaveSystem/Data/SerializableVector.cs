// using System;
// using UnityEngine;

// namespace SaveSystem.Data
// {
//     [Serializable]
//     public struct SerializableVector3
//     {
//         public float x;
//         public float y;
//         public float z;
        
//         public SerializableVector3(float x, float y, float z)
//         {
//             this.x = x;
//             this.y = y;
//             this.z = z;
//         }
        
//         public SerializableVector3(Vector3 vector)
//         {
//             x = vector.x;
//             y = vector.y;
//             z = vector.z;
//         }
        
//         public Vector3 ToVector3()
//         {
//             return new Vector3(x, y, z);
//         }
        
//         public static implicit operator Vector3(SerializableVector3 sVector)
//         {
//             return sVector.ToVector3();
//         }
        
//         public static implicit operator SerializableVector3(Vector3 vector)
//         {
//             return new SerializableVector3(vector);
//         }
//     }
// }

using System;
using UnityEngine;

namespace SaveSystem.Data
{
    [Serializable]
    public class SerializableVector3
    {
        public float x;
        public float y;
        public float z;
        
        // Constructeurs
        public SerializableVector3() { }
        
        public SerializableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public SerializableVector3(Vector3 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }
        
        // Conversions implicites
        public static implicit operator Vector3(SerializableVector3 vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }
        
        public static implicit operator SerializableVector3(Vector3 vector)
        {
            return new SerializableVector3(vector);
        }
        
        // Méthodes utilitaires
        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }
    }
}