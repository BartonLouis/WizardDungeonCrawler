using Managers;
using UnityEditor;
using UnityEngine;


namespace Editors {
    [CustomEditor(typeof(InputManager))]
    public class InputManagerEditor : Editor {

        InputManager _object;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (_object == null) {
                _object = target as InputManager;
            }
            GUILayout.Space(50);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Reconnect", GUILayout.Width(200), GUILayout.Height(100))) {
                _object.Reconnect();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

        }
    }
}