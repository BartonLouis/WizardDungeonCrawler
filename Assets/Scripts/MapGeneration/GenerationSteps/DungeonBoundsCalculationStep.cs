using UnityEngine;

namespace DungeonGeneration {
    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Dungeon Bounds Generation Step")]
    public class DungeonBoundsCalculationStep : AbstractGenerationStep {
        [Header("Settings")]
        [SerializeField] int _falloffRadius;
        [SerializeField] int _safeZoneSize;

        public override void Generate(Dungeon dungeon) {

            int max = 0;
            foreach (RoomInfo room in dungeon.Rooms) {
                int distanceSqrd = (int)(room.bounds.center.x * room.bounds.center.x + room.bounds.center.y * room.bounds.center.y);
                max = distanceSqrd > max ? distanceSqrd : max;
            }


            int radius = (int)Mathf.Sqrt(max);
            int squareSize = Mathf.CeilToInt(2 * (radius + _falloffRadius + _safeZoneSize));
            int width = squareSize;
            int height = squareSize;
            TileInfo[] tiles = new TileInfo[width * height];

            int halfWidth = width / 2;
            int halfHeight = height / 2;
            for (int x = -halfWidth; x < halfWidth; x++) {
                for (int y = -halfHeight; y < halfHeight; y++) {
                    tiles[(width * (y + halfHeight)) + x + halfWidth] = new TileInfo() {
                        x = x,
                        y = y,
                        layer = TileLayer.Floor
                    };
                }
            }
            dungeon.SetMap(tiles, width, height, radius, _falloffRadius, _safeZoneSize);
        }
    }
}