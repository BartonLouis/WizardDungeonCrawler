using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
[InitializeOnLoad]
public partial class AdvancedHierarchyDisplay {

    static bool _hierarchyHasFocus = false;
    static EditorWindow _hierarchyEditorWindow;


    static AdvancedHierarchyDisplay() {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGui;
        EditorApplication.update += OnEditorUpdate;
    }

    private static void OnEditorUpdate() {
        if (_hierarchyEditorWindow == null)
            _hierarchyEditorWindow = EditorWindow.GetWindow(Type.GetType("UnityEditor.SceneHierarchyWindow,UnityEditor"));

        _hierarchyHasFocus = EditorWindow.focusedWindow != null &&
            EditorWindow.focusedWindow == _hierarchyEditorWindow;
    }

    static void DrawActivationToggle(Rect selectionRect, GameObject gameObject) {
        if (!gameObject.TryGetComponent<Canvas>(out Canvas canvas)) {
            return;
        }
        Rect toggleRect = new Rect(selectionRect);
        toggleRect.x -= 27f;
        toggleRect.width = 13f;
        bool active = EditorGUI.Toggle(toggleRect, canvas.enabled);
        if (active != canvas.enabled) {
            Undo.RecordObject(gameObject, "Changing active state of game object");
            canvas.enabled = active;
            if (!EditorApplication.isPlaying) {
                EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }
        }
    }

    static void OnHierarchyWindowItemOnGui(int instanceID, Rect selectionRect) {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj == null) return;

        DrawActivationToggle(selectionRect, obj);

        if (PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj) != null) return;

        Component[] components = obj.GetComponents<Component>();
        if (components == null || components.Length == 0) return;

        // Use first component as highest priority
        Component component = components.Length > 1 ? components[1] : components[0];
        if (component == null) component = components[0];
        Type type = component.GetType();

        GUIContent content = EditorGUIUtility.ObjectContent(component, type);
        content.text = null;
        content.tooltip = type.Name;

        if (content.image == null) return;

        bool isSelected = Selection.instanceIDs.Contains(instanceID);
        bool isHovering = selectionRect.Contains(Event.current.mousePosition);

        Color color = UnityEditorBackgroundColor.Get(isSelected, isHovering, _hierarchyHasFocus);
        Rect backgroundRect = selectionRect;
        backgroundRect.width = 18.5f;
        EditorGUI.DrawRect(backgroundRect, color);
        EditorGUI.LabelField(selectionRect, content);
    }
}
