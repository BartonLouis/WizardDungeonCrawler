using System.Linq;
using UnityEditor;
using UnityEngine;
using Utils;

namespace DungeonGeneration {

    [CustomEditor(typeof(CorridorGenerationStep))]
    public class CorridorGenerationStepEditor : Editor {
        CorridorGenerationStep _target;

        public override void OnInspectorGUI() {
            _target = target as CorridorGenerationStep;
            base.OnInspectorGUI();
            if (GUILayout.Button("Reset Grid")) {
                _target.roomConnections.ResetGrid();
            }

            var enumValues = EnumUtils.GetValues<RoomType>().ToArray();
        }
    }
}