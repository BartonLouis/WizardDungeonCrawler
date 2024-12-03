using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration {
    [RequireComponent(typeof(TilemapVisualiser))]
    public class DungeonGenerator : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] int _seed;
        [SerializeField] Vector2Int _center;
        [SerializeField] int _width;
        [SerializeField] int _height;
        [SerializeField] int _border;

        [Header("Generation Steps")]
        [SerializeField] List<AbstractGenerationStep> _generationSteps;

        TilemapVisualiser _visualiser;

        public void Generate(int seed) {
            _seed = seed;
            Generate();
        }

        public void Generate() {
            DungeonInfo dungeon = new DungeonInfo(_center, _width, _height, _border, _seed);
            foreach (var step in _generationSteps) {
                dungeon = step.Generate(dungeon);
            }

            _visualiser = GetComponent<TilemapVisualiser>();
            _visualiser.Clear();
            foreach(var tile in dungeon) {
                _visualiser.PaintSingleTile(dungeon.Center + new Vector2Int(tile.x - dungeon.Width / 2, tile.y - dungeon.Height / 2), tile.layer);
            }
        }

    }

    public interface IGenerationStep {
        public DungeonInfo Generate(DungeonInfo dungeon);
    }
}