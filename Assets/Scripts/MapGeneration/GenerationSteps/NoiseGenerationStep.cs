using UnityEngine;
using Utils;
using Random = System.Random;

namespace DungeonGeneration {

    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Noise Generation Step")]
    public class NoiseGenerationStep : AbstractGenerationStep {

        [Header("Settings")]
        [Range(0f, 1f)]
        [SerializeField] float _wallDensity;


        public override DungeonInfo Generate(DungeonInfo dungeon) {
            Random random = new(dungeon.Seed);

            foreach(var tile in dungeon) {
                if (dungeon.IsWithinBorder(tile.x, tile.y)) {
                    dungeon[tile.x, tile.y] = new TileInfo(tile, random.NextDouble() < _wallDensity ? TileLayer.Wall : TileLayer.Floor);
                }
            }

            return dungeon;
        }
    }
}