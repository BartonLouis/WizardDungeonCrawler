using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation {
    [CreateAssetMenu(menuName = "Data/Map Generation Settings")]
    public class MapGenerationSettings : ScriptableObject {
        [Header("Steps")]
        [SerializeField] MapGenerationStep[] _steps;
        NoopGenerationStep _default = new();

        public IEnumerable<IMapGenerationStep> Steps {
            get {
                foreach(var step in _steps) yield return step;
            }
        }

        public IMapGenerationStep this[int index] {
            get {
                if(index < _steps.Length) return _steps[index];
                return _default;
            }
        }


        class NoopGenerationStep : IMapGenerationStep {
            public bool ApplyStep(Map map, Random random) => false;
            public void Init() { }
        }
    }
}