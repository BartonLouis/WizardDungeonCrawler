using System.Collections.Generic;
using UnityEngine;

namespace Managers {
#if UNITY_EDITOR
    public class Logging {
        public static readonly Dictionary<LogLevel, Color> colours = new() {
            { LogLevel.Debug, Color.white },
            { LogLevel.Warning, Color.yellow },
            { LogLevel.Error, Color.red }
        };


        public static void Log(Object sender, string message, LogLevel logLevel = LogLevel.Debug) {
            Debug.Log($"<b><color=#{ColorUtility.ToHtmlStringRGBA(colours[logLevel])}>{sender.name}: </color></b> {message}");
        }

        public static void Log(string sender, string message, LogLevel logLevel = LogLevel.Debug) {
            Debug.Log($"<b><color=#{ColorUtility.ToHtmlStringRGBA(colours[logLevel])}>{sender}: </color></b> {message}");
        }

    }

    public enum LogLevel {
        Debug,
        Warning,
        Error
    }
#endif
}