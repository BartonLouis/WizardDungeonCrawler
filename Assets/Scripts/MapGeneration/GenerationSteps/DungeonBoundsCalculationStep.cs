using UnityEngine;

namespace DungeonGeneration {
    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Dungeon Bounds Generation Step")]
    public class DungeonBoundsCalculationStep : AbstractGenerationStep {
        [Header("Settings")]
        [SerializeField] int _falloffRadius;
        [SerializeField] int _safeZoneSize;

        public override void Generate(Dungeon dungeon) {
            int radius = 0;
            foreach (RoomInfo room in dungeon.Rooms) {
                float dist = Vector2.Distance(dungeon.Center, room.bounds.center);
                //dist += Mathf.Sqrt(room.bounds.size.x * room.bounds.size.x + room.bounds.size.y * room.bounds.size.y);
                if (dist > radius) {
                    radius = Mathf.CeilToInt(dist);
                }
            }

            int squareSize = Mathf.CeilToInt(2 * (radius + _falloffRadius + _safeZoneSize));
            int width = squareSize;
            int height = squareSize;
            TileInfo[] tiles = new TileInfo[width * height];
            for (int x = -width / 2; x < width / 2; x++) {
                for (int y = -height / 2; y < height / 2; y++) {
                    tiles[(width * (y + height / 2)) + x + width / 2] = new TileInfo() {
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