using UnityEngine;

namespace CoreFramework
{
    public class JsonSerializer : ICustomSerializer
    {
        public T Deserialize<T>(string data) => JsonUtility.FromJson<T>(data);

        public string Serialize<T>(T val) => JsonUtility.ToJson(val);
    }
}