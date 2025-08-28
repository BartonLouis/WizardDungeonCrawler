using UnityEngine;

namespace MapGeneration {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Null Map Generation Step", order = 0)]
    public class MapGenerationStep : ScriptableObject, IMapGenerationStep {
        public virtual Map ApplyStep(Map map) { return map; }
    }

    public interface IMapGenerationStep {
        Map ApplyStep(Map map);
    }
}