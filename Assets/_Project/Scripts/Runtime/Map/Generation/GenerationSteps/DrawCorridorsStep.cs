using Louis.CustomPackages.Logging;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation.GenerationSteps {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Draw Corridors Step", order = 11)]
    public class DrawCorridorsStep : MapGenerationStep {
        [SerializeField] int _corridorSize;

        public override void ApplyStep(Map map, Random random) {
            foreach(var corridor in map.corridors) {
                DrawCorridor(map, corridor);
            }
            map.changedFlag = true;
        }


        void DrawCorridor(Map map, Corridor corridor) {
            for (int i = 0; i < corridor.positions.Length - 1; i++) {
                DrawSegment(map, corridor.positions[i], corridor.positions[i + 1]);
            }
        }

        void DrawSegment(Map map, Vector2 start, Vector2 end) {
            int x0 = (int)start.x;
            int y0 = (int)start.y;
            int x1 = (int)end.x;
            int y1 = (int)end.y;
            int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2;
            for(; ; ) {
                BrushOnPoint(map, new Vector2Int(x0, y0));

                if(x0 == x1 && y0 == y1) break;

                e2 = 2 * err;

                // horizontal step?
                if(e2 > dy) {
                    err += dy;
                    x0 += sx;
                }

                // vertical step?
                else if(e2 < dx) {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        void BrushOnPoint(Map map, Vector2Int p) {
            int min = Mathf.FloorToInt(_corridorSize / 2f);
            int max = Mathf.CeilToInt(_corridorSize / 2f);
            int halfMapWidth = map.Size.x / 2;
            int halfMapHeight = map.Size.y / 2;
            int halfSize = _corridorSize / 2;
            for(int x = p.x - min; x < p.x + max; x++) {
                for(int y = p.y - min; y < p.y + max; y++) {
                    int newX = x + halfMapWidth;
                    int newY = map.Size.y - (y + halfMapHeight);
                    map[newX, newY] = TileType.Floor;

                }
            }
        }
    }
}