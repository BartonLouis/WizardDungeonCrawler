using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration {
    [CreateAssetMenu(menuName = "Data/Map Generation Settings")]
    public class MapGenerationSettings : ScriptableObject {
        [Header("Steps")]
        [SerializeField] MapGenerationStep[] _steps;

        public IEnumerable<IMapGenerationStep> Steps {
            get {
                foreach(var step in _steps) yield return step;
            }
        }
    }
}