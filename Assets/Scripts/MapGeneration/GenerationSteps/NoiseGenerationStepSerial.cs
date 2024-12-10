using UnityEngine;
using Random = System.Random;

namespace DungeonGeneration {

    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Noise Generation Serial Step")]
    public class NoiseGenerationStepSerial : AbstractGenerationStep {

        [Header("Settings")]
        [Range(0f, 1f)]
        [SerializeField] float _wallDensity;


        public override void Generate(Dungeon dungeon) {
            Random random = new(dungeon.Seed);
            float innerSqr = dungeon.Radius * dungeon.Radius;
            float outerSqr = (dungeon.Radius + dungeon.FalloffRadius) * (dungeon.Radius + dungeon.FalloffRadius);
            int minX = (int)(dungeon.Center.x - dungeon.Radius - dungeon.FalloffRadius);
            int maxX = (int)(dungeon.Center.x + dungeon.Radius + dungeon.FalloffRadius);
            int minY = (int)(dungeon.Center.y - dungeon.Radius - dungeon.FalloffRadius);
            int maxY = (int)(dungeon.Center.y + dungeon.Radius + dungeon.FalloffRadius);
            for (int x = minX; x <= maxX; x++) {
                for (int y = minY; y <= maxY; y++) {
                    TileInfo tile = dungeon[x, y];
                    float distSqr = tile.x * tile.x + tile.y * tile.y; if (distSqr < innerSqr) {
                        tile.layer = (float)random.NextDouble() < _wallDensity ? TileLayer.Wall : TileLayer.Floor;
                    } else if (distSqr < outerSqr) {
                        float lerpedThreshold = Mathf.Lerp(_wallDensity, 1, (distSqr - innerSqr) / outerSqr);
                        tile.layer = (float)random.NextDouble() < lerpedThreshold ? TileLayer.Wall : TileLayer.Floor;
                    } else {
                        tile.layer = TileLayer.Wall;
                    }
                    dungeon[x, y] = tile;
                }
            }
        }
    }
}