using System;
using Newtonsoft.Json;

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

                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                return default;
            }
        }
        
        public static string SerializeToString<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}