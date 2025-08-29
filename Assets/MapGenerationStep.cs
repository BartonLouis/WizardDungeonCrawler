using System;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Null Map Generation Step", order = 0)]
    public abstract class MapGenerationStep : ScriptableObject, IMapGenerationStep {
        public abstract bool ApplyStep(Map map, Random random);
        public virtual void Init() { }
    }

    public interface IMapGenerationStep {
        bool ApplyStep(Map map, Random random);
        void Init();
    }
}