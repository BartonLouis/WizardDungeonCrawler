using UnityEngine;

namespace DungeonGeneration {

    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Draw Corridors Step")]
    public class DrawCorridorStep : AbstractGenerationStep {

        [Header("Settings")]
        [SerializeField] int _corridorSize;
        Dungeon _dungeon;


        public override void Generate(Dungeon dungeon) {
            _dungeon = dungeon;
            foreach (CorridorInfo corridor in dungeon.Corridors) {
                DrawCorridor(corridor);
            }
        }

        public void DrawCorridor(CorridorInfo corridor) {
            RoomInfo startRoom = _dungeon.Rooms[corridor.startRoomIndex];
            RoomInfo endRoom = _dungeon.Rooms[corridor.endRoomIndex];
            Vector2 startDoor = corridor.startDoor;
            Vector2 endDoor = corridor.endDoor;

            Bresenham(startRoom.bounds.center, startDoor);
            Bresenham(startDoor, endDoor);
            Bresenham(endDoor, endRoom.bounds.center);
        }

        void Bresenham(Vector2 start, Vector2 end) {
            // Bresenham algorithm to rasteurize line along corridor
            int x0 = (int)start.x;
            int y0 = (int)start.y;
            int x1 = (int)end.x;
            int y1 = (int)end.y;
            int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2;
            for (; ; ) {
                BrushOnPoint(new Vector2Int(x0, y0));

                if (x0 == x1 && y0 == y1) break;

                e2 = 2 * err;

                // horizontal step?
                if (e2 > dy) {
                    err += dy;
                    x0 += sx;
                }

                // vertical step?
                else if (e2 < dx) {
                    err += dx;
                    y0 += sy;
                }
            }
            // Debug.DrawLine(start, end, Color.green, 5);
        }

        void BrushOnPoint(Vector2Int point) {
            int min = Mathf.FloorToInt(-_corridorSize / 2f);
            int max = Mathf.FloorToInt(_corridorSize / 2f);
            for (int x = min; x < max; x++) {
                for (int y = min; y < max; y++) {
                    Vector2Int newPoint = point + new Vector2Int(x, y);
                    TileInfo tile = _dungeon[newPoint.x, newPoint.y];
                    tile.layer = TileLayer.Floor;
                    _dungeon[newPoint.x, newPoint.y] = tile;
                }
            }
        }
    }
}