using DungeonGeneration;
using UnityEditor;
using UnityEngine;

namespace Editors {
    [CustomEditor(typeof(DungeonGenerator))]
    public class DungeonGeneratorEditor : Editor {

        DungeonGenerator _generator;


        public override void OnInspectorGUI() {
            if (_generator == null) {
                _generator = target as DungeonGenerator;
            }
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate")) {
                _generator.Generate();
            }
            if (GUILayout.Button("Generate New")) {
                int seed = Mathf.FloorToInt(Random.Range(0f, 1f) * int.MaxValue);
                _generator.Generate(seed);
            }
        }

    }
}