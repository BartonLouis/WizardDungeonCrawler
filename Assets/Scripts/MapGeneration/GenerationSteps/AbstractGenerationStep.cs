using UnityEngine;

namespace DungeonGeneration {
    public abstract class AbstractGenerationStep : ScriptableObject, IGenerationStep {
        public abstract void Generate(DungeonInfo dungeon);
    }
}