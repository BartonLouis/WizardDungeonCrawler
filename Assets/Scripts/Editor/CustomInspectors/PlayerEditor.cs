using Gameplay.Player.Stats;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Editors {
    [CustomEditor(typeof(PlayerStatsManager))]
    public class PlayerEditor : Editor {
        PlayerStatsManager _player;
        bool showPrimary = true;
        bool showSecondary = true;

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (_player == null) {
                _player = target as PlayerStatsManager;
            }
            _player.GenerateStats();
            GUILayout.Space(25);
            showPrimary = EditorGUILayout.Foldout(showPrimary, "Primary Stats");
            if (showPrimary) {
                foreach (var stat in EnumUtils.GetValues<PrimaryStatTag>()) {
                    if (stat == PrimaryStatTag.None) continue;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"\t{stat.ToString()[..3]}:", GUILayout.Width(200));
                    GUILayout.Label($"{_player[stat].Value}");
                    GUILayout.EndHorizontal();
                }
            }
            showSecondary = EditorGUILayout.Foldout(showSecondary, "Secondary Stats");
            if (showSecondary) {
                foreach (var stat in EnumUtils.GetValues<SecondaryStatTag>()) {
                    if (stat == SecondaryStatTag.None) continue;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"\t{stat}:", GUILayout.Width(200));
                    GUILayout.Label($"{_player[stat].Value} {_player[stat].Unit}");
                    GUILayout.EndHorizontal();
                }
            }
        }
    }
}