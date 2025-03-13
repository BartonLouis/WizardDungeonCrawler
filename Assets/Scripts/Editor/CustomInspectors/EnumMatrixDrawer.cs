using CustomTypes;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEditor {
    [CustomPropertyDrawer(typeof(EnumMatrix))]
    public class EnumMatrixDrawer : PropertyDrawer {

        const int checkboxSize = 16;
        const int indent = 75;

        static class Styles {
            public static readonly GUIStyle rightLabel = new GUIStyle("RightLabel");
            public static readonly GUIStyle hoverStyle = GetHoverStyle();
        }

        private static Color transparentColor = new Color(1, 1, 1, 0);
        private static Color highlightColor = EditorGUIUtility.isProSkin ? new Color(1, 1, 1, 0.2f) : new Color(0, 0, 0, 0.2f);

        public delegate bool GetValueFunc(int layerA, int layerB);
        public delegate void SetValueFunc(int layerA, int layerB, bool val);

        // Get the styled used when hovering over the rows/columns.
        public static GUIStyle GetHoverStyle() {
            GUIStyle style = new GUIStyle(EditorStyles.label);

            var texNormal = new Texture2D(1, 1) { alphaIsTransparency = true };
            texNormal.SetPixel(1, 1, transparentColor);
            texNormal.Apply();

            var texHover = new Texture2D(1, 1) { alphaIsTransparency = true };
            texHover.SetPixel(1, 1, highlightColor);
            texHover.Apply();

            style.normal.background = texNormal;
            style.hover.background = texHover;

            return style;
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            GUILayout.Label(label);
            SerializedProperty matrix = property.FindPropertyRelative("matrix");
            SerializedProperty labels = property.FindPropertyRelative("valueNames");

            List<string> colLabels = new List<string>();
            int longestLabel = 0;
            for (int i = 0; i < labels.arraySize; i++) {
                string col = labels.GetArrayElementAtIndex(i).stringValue;
                colLabels.Add(col);
                var textDimensions = GUI.skin.label.CalcSize(new GUIContent(col));
                if (textDimensions.x > longestLabel) {
                    longestLabel = (int)textDimensions.x + 2;
                }
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            Vector2 pivot = new Vector2(position.x + indent + longestLabel, position.y + colLabels.Count * EditorGUIUtility.singleLineHeight);
            GUIUtility.RotateAroundPivot(90, pivot);
            GUILayout.BeginVertical();
            foreach(string colLabel in colLabels) {
                GUILayout.Label(colLabel, GUILayout.Width(longestLabel));
            }
            GUILayout.EndVertical();
            GUIUtility.RotateAroundPivot(-90, pivot);
            GUILayout.EndHorizontal();


            GUILayout.BeginVertical();
            for (int rowIndex = 0; rowIndex < matrix.arraySize; rowIndex++) {
                SerializedProperty row = matrix.GetArrayElementAtIndex(rowIndex).FindPropertyRelative("row");
                GUILayout.BeginHorizontal();
                GUILayout.Label(labels.GetArrayElementAtIndex(rowIndex).stringValue, GUILayout.Width(100));
                for (int i = 0; i < row.arraySize - rowIndex; i++) {
                    var value = row.GetArrayElementAtIndex(i);
                    value.boolValue = GUILayout.Toggle(value.boolValue, GUIContent.none, GUILayout.Width(EditorGUIUtility.singleLineHeight));
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}