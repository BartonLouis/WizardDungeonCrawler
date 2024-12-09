using DungeonGeneration;
using Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils {
    public static class DungeonInfoExtensions {

        public static bool IsWithinBorder(this Dungeon dungeon, int x, int y) {
            return (x >= dungeon.Border && x < dungeon.Width - dungeon.Border && y >= dungeon.Border && y < dungeon.Height - dungeon.Border);
        }

        public static IEnumerable<Vector2Int> IterateThroughBounds(this BoundsInt bounds) {
            for (int x = bounds.min.x; x < bounds.max.x; x++) {
                for (int y = bounds.min.y; y < bounds.max.y; y++) {
                    yield return new Vector2Int(x, y);
                }
            }
        }

        public static Vector2 GenerateDoor(this RoomInfo room, Side side) {
            float halfWidth = room.bounds.size.x / 2f;
            float halfHeight = room.bounds.size.y / 2f;
            return room.bounds.center + side switch {
                Side.Top => new Vector3(0, halfHeight),
                Side.Bottom => new Vector3(0, -halfHeight),
                Side.Left => new Vector3(-halfWidth, 0),
                Side.Right => new Vector3(halfWidth, 0),
                _ => Vector3.zero,
            };
        }

        public static void DrawRoom(this Dungeon dungeon, RoomInfo room) {
            BoundsInt bounds = room.bounds;
            int margin = room.margin;
            int border = room.border;
            for (int x = bounds.min.x + margin; x < bounds.max.x - margin; x++) {
                for (int y = bounds.min.y + margin; y < bounds.max.y - margin; y++) {
                    TileInfo info = dungeon[x, y];
                    TileLayer layer = (x < bounds.min.x + border + margin
                        || x > bounds.max.x - border - margin - 1
                        || y < bounds.min.y + border + margin
                        || y > bounds.max.y - border - margin - 1)
                        ? TileLayer.Wall : TileLayer.Floor;
                    dungeon[x, y] = new TileInfo(info, layer);
                }
            }
        }

        public static bool AnyIntersect(this IReadOnlyList<RoomInfo> rooms) {
            return BoundsIntExtensions.AnyIntersect(rooms.Select(r => r.bounds).ToList());
        }

        public static float Length(this CorridorInfo corridor, Dungeon dungeon) {
            RoomInfo r1 = dungeon.Rooms[corridor.startRoomIndex];
            RoomInfo r2 = dungeon.Rooms[corridor.endRoomIndex];
            return Vector2.Distance(r1.bounds.center, r2.bounds.center);
        }
    }
}