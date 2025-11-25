using Stats;
using UnityEditor;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(StatValue))]
public class StatValueDrawer : PropertyDrawer {
    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
        // Root container (horizontal)
        var container = new VisualElement();
        container.style.flexDirection = FlexDirection.Row;
        container.style.flexGrow = 1;
        container.style.flexShrink = 1;

        // Child properties
        var typeProp = property.FindPropertyRelative("type");
        var valueProp = property.FindPropertyRelative("value");

        // Enum field (compact, minimal grow)
        var enumField = new EnumField((StatType)typeProp.enumValueIndex);
        enumField.style.flexBasis = 100;   // fixed-ish width
        enumField.style.flexGrow = 0;      // does NOT expand
        enumField.style.flexShrink = 0;

        enumField.RegisterValueChangedCallback(evt => {
            typeProp.enumValueIndex = (int)(StatType)evt.newValue;
            property.serializedObject.ApplyModifiedProperties();
        });

        // Integer field (expand to fill remaining space)
        var intField = new IntegerField()
    {
            value = valueProp.intValue
        };
        intField.style.flexGrow = 1;       // expands to fill space
        intField.style.flexShrink = 1;
        intField.style.marginLeft = 6;

        intField.RegisterValueChangedCallback(evt => {
            valueProp.intValue = evt.newValue;
            property.serializedObject.ApplyModifiedProperties();
        });

        // Add to container
        container.Add(enumField);
        container.Add(intField);

        return container;
    }

}
