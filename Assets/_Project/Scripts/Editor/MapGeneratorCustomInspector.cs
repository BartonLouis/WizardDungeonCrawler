using UnityEditor;
using UnityEngine.UIElements;

namespace Map.Generation {
    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorCustomInspector : Editor {
        public VisualTreeAsset _visualTreeAsset;
        MapGenerator _generator;

        private void OnEnable() {
            _generator = (MapGenerator)target;
        }

        public override VisualElement CreateInspectorGUI() {
            VisualElement root = new();
            _visualTreeAsset.CloneTree(root);

            Button generateButton = root.Q<Button>("GenerateButton");
            generateButton.RegisterCallback<ClickEvent>(OnClick);
            return root;
        }

        void OnClick(ClickEvent evnt) {
            _generator.RunStep();
        }
    }
}