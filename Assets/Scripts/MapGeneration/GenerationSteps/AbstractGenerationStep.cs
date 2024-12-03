using UnityEngine;

namespace DungeonGeneration {
    public abstract class AbstractGenerationStep : ScriptableObject, IGenerationStep {
        public abstract DungeonInfo Generate(DungeonInfo dungeon);
    }
}