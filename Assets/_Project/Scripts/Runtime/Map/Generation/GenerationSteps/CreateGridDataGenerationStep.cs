using UnityEngine;
using Random = System.Random;

namespace Map.Generation.GenerationSteps {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Create Grid Data Step", order = 8)]
    internal class CreateGridDataGenerationStep : MapGenerationStep {
        [SerializeField] int _mapSafeBounds = 10;

        public override void ApplyStep(Map map, System.Random random) {
            int minX = 0, 
                maxX = 0, 
                minY = 0, 
                maxY = 0;
            foreach(var room in map.rooms) {
                var bounds = room.SafeBounds;

                maxX = Mathf.Max(maxX, bounds.maxX);
                minX = Mathf.Min(minX, bounds.minX);
                maxY = Mathf.Max(maxY, bounds.maxY);
                minY = Mathf.Min(minY, bounds.minY);
            }

            int x = Mathf.Max(Mathf.Abs(minX), maxX);
            int y = Mathf.Max(Mathf.Abs(minY), maxY);
            int width = 2 * (x + _mapSafeBounds);
            int height = 2 * (y + _mapSafeBounds);

            map.Size = new Vector2Int(width, height);
            for(int i = 0; i < width; i++) {
                for(int j = 0; j < height; j++) {
                    map[i, j] = TileType.Wall;
                }
            }
        }
    }
}