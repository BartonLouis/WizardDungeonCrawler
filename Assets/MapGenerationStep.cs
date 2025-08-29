using UnityEngine;
using Random = System.Random;

namespace Map.Generation {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Null Map Generation Step", order = 0)]
    public abstract class MapGenerationStep : ScriptableObject, IMapGenerationStep {
        public abstract void ApplyStep(Map map, Random random);
    }

    public interface IMapGenerationStep {
        void ApplyStep(Map map, Random random);
    }
}