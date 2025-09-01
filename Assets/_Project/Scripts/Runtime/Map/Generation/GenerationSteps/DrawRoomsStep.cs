using UnityEngine;
using Random = System.Random;

namespace Map.Generation.GenerationSteps {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Draw Rooms Step", order = 10)]
    internal class DrawRoomsStep : MapGenerationStep {
        public override void ApplyStep(Map map, Random random) {
            int halfWidth = map.Size.x / 2;
            int halfHeight = map.Size.y / 2;
            int height = map.Size.y;

            foreach(var room in map.rooms) {
                foreach((int x, int y) in room.InteriorPositions) {
                    map[x + halfWidth, height - (y + halfHeight)] = TileType.Floor;
                }
                foreach((int x, int y) in room.BorderPositions) {
                    map[x + halfWidth, height - (y + halfHeight)] = TileType.Wall;
                }
            }
            map.changedFlag = true;
        }
    }
}