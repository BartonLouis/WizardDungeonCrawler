using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration {
    [RequireComponent(typeof(TilemapVisualiser))]
    public class DungeonGenerator : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] int _seed;

        [Header("Generation Steps")]
        [SerializeField] List<AbstractGenerationStep> _generationSteps;

        TilemapVisualiser _visualiser;

        public void Generate(int seed) {
            _seed = seed;
            Generate();
        }

        public void Generate() {
            DungeonInfo dungeon = new DungeonInfo(_seed, Vector2Int.zero);
            foreach (var step in _generationSteps) {
                step.Generate(dungeon);
            }

            _visualiser = GetComponent<TilemapVisualiser>();
            _visualiser.Clear();
            foreach(var tile in dungeon) {
                _visualiser.PaintSingleTile(new Vector2Int(tile.x, tile.y), tile.layer);
            }
        }

    }

    public interface IGenerationStep {
        public void Generate(DungeonInfo dungeon);
    }
}