using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Louis.Patterns.ServiceLocator {
    public static class ServiceLocatorUtil {
        public static IReadOnlyList<Type> ServiceTypes { get; set; }
        public static IReadOnlyList<Type> DefinitionTypes { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize() {
            ServiceTypes = PredefinedAssemblyUtil.GetTypes(typeof(IService));
            DefinitionTypes = InitializeAllDefinitions();
        }


        static List<Type> InitializeAllDefinitions() {
            List<Type> definitionTypes = new();
            var typedef = typeof(ServiceDefinition<>);
            foreach (var serviceType in ServiceTypes) {
                var definitionType = typedef.MakeGenericType(serviceType);
                definitionTypes.Add(definitionType);
            }
            return definitionTypes;
        }



#if UNITY_EDITOR
        public static PlayModeStateChange PlayModeState { get; set; }
        [InitializeOnLoadMethod]
        public static void InitializeEditor() {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        static void OnPlayModeStateChanged(PlayModeStateChange state) {
            PlayModeState = state;
            if (state == PlayModeStateChange.ExitingPlayMode)
                ClearAllDefinitions();
        }
#endif


        public static void ClearAllDefinitions() {
            if (DefinitionTypes == null) return;
            for (int i = 0; i < DefinitionTypes.Count; i++) {
                var definitionType = DefinitionTypes[i];
                var clearMethod = definitionType.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
                //clearMethod.Invoke(null, null);
            }
        }
    }
}