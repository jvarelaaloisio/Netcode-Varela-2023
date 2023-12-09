using UnityEngine;

namespace Core.Extensions
{
    public static class ObjectExtensions
    {
        public static void Log(this UnityEngine.Object writer, object message)
            => Debug.Log(GetFormattedMessage(writer, message), writer);
        public static void LogError(this UnityEngine.Object writer, object message)
            => Debug.LogError(GetFormattedMessage(writer, message), writer);
        public static void LogWarning(this UnityEngine.Object writer, object message)
            => Debug.LogWarning(GetFormattedMessage(writer, message), writer);
        private static string GetFormattedMessage(Object writer, object message) => $"{writer.name}: {message}";
    }
}
