using Map;
using Map.Generation;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RoomAttractionMatrix))]
public class SymmetricRoomAttractionMatrixDrawer : PropertyDrawer {
    private const float CellWidth = 50f;
    private const float CellHeight = 18f;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        int size = System.Enum.GetValues(typeof(RoomType)).Length;
        return (size + 2) * CellHeight; // rows + header + label
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);

        var valuesProp = property.FindPropertyRelative("values");
        int size = System.Enum.GetValues(typeof(RoomType)).Length;
        var types = System.Enum.GetValues(typeof(RoomType));

        // Draw label
        Rect labelRect = new Rect(position.x, position.y, position.width, CellHeight);
        EditorGUI.LabelField(labelRect, label);

        float startY = position.y + CellHeight;
        float startX = position.x + 80f; // space for row labels

        // Column headers (left to right)
        for(int col = 0; col < size; col++) {
            Rect headerRect = new Rect(startX + col * CellWidth, startY, CellWidth, CellHeight);
            EditorGUI.LabelField(headerRect, types.GetValue(col).ToString(), EditorStyles.boldLabel);
        }

        int index = 0;

        // Rows (drawn top to bottom, so invert row index)
        for(int row = size - 1; row >= 0; row--) {
            float y = startY + (size - row) * CellHeight;

            // Row label
            Rect rowLabelRect = new Rect(position.x, y, 80f, CellHeight);
            EditorGUI.LabelField(rowLabelRect, types.GetValue(row).ToString(), EditorStyles.boldLabel);

            for(int col = 0; col <= row; col++) // upper-left triangle
            {
                Rect cellRect = new Rect(startX + col * CellWidth, y, CellWidth, CellHeight);
                var cellProp = valuesProp.GetArrayElementAtIndex(index);
                cellProp.floatValue = EditorGUI.FloatField(cellRect, GUIContent.none, cellProp.floatValue);
                index++;
            }
        }

        EditorGUI.EndProperty();
    }
}
