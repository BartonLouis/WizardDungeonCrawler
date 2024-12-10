using Gameplay.Rooms;
using Louis.Patterns.ServiceLocator;
using UnityEngine;
using Utils;

namespace DungeonGeneration {
    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Draw Rooms Step")]
    public class DrawRoomsStep : AbstractGenerationStep {
        [Header("Settings")]
        [SerializeField] RoomCollider _prefab;
        Dungeon _dungeon;

        public override void Generate(Dungeon dungeon) {
            _dungeon = dungeon;
            ServiceLocator.TryGetService<IDungeonCreationService>(out var generatorService);
            Transform parent = generatorService != null ? generatorService.Transform : null;
            foreach (var room in dungeon.Rooms) {
                RoomCollider collider = Instantiate(_prefab);
                collider.transform.parent = parent;
                collider.Init(room);
            }

            foreach(RoomInfo room in dungeon.Rooms) {
                DrawRoom(room);
            }
        }

        public void DrawRoom(RoomInfo room) {
            BoundsInt bounds = room.bounds;
            int margin = room.margin;
            int border = room.border;
            int minX = bounds.min.x + margin;
            int maxX = bounds.max.x - margin;
            int minY = bounds.min.y + margin;
            int maxY = bounds.max.y - margin;
            int minXBorder = bounds.min.x + border + margin;
            int maxXBorder = bounds.max.x - border - margin - 1;
            int minYBorder = bounds.min.y + border + margin;
            int maxYBorder = bounds.max.y - border - margin - 1;
            for (int x = minX; x < maxX; x++) {
                for (int y = minY; y < maxY; y++) {
                    TileInfo info = _dungeon[x, y];
                    TileLayer layer = (x < minXBorder
                        || x > maxXBorder
                        || y < minYBorder
                        || y > maxYBorder)
                        ? TileLayer.Wall : TileLayer.Floor;
                    _dungeon[x, y] = new TileInfo(info, layer);
                }
            }
        }
    }
}