using System;
using UnityEngine;

namespace Helpers
{
    public class JsonHelper
    {
        public static T DeserializeFromString<T>(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json))
                    return default;

                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception ex)
            {
                return default;
            }
        }
        
        public static string SerializeToString<T>(T data)
        {
            return JsonUtility.ToJson(data);
        }
    }
}