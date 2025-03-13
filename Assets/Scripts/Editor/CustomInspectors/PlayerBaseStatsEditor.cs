using Gameplay.Player.Stats;
using Managers;
using UnityEditor;

namespace Editors {
    [CustomEditor(typeof(InputManager))]
    public class PlayerBaseStatsEditor : Editor {

        PlayerBaseStats _object;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }
    }
}