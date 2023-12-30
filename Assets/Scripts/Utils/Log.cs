using System;
using UnityEngine;

namespace Utils {
    public class Log {
        public readonly bool enabled;
        readonly string tag;

        public Log(string tag, bool enabled = true) {
            this.tag = tag;
            this.enabled = enabled;
        }

        public Log(Type type, bool enabled = true) : this(type.Name, enabled) {}

        // ReSharper disable Unity.PerformanceAnalysis
        public void log(object message, object tag = null) {
            if (enabled) Debug.Log($"{tag ?? this.tag}: {message}");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void warn(object message, object tag = null) {
            Debug.LogWarning($"{tag ?? this.tag}: {message}");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void error(object message, object tag = null) {
            Debug.LogError($"{tag ?? this.tag}: {message}");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static void log(string tag, object message) {
            Debug.Log($"{tag}: {message}");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static void warn(string tag, object message) {
            Debug.LogWarning($"{tag}: {message}");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static void error(string tag, object message) {
            Debug.LogError($"{tag}: {message}");
        }
    }
}