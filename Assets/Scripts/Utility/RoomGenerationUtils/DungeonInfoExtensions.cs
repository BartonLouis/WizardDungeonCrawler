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

        public static Vector2 GenerateDoor(this RoomInfo room, DoorSide side) {
            float halfWidth = room.bounds.size.x / 2f;
            float halfHeight = room.bounds.size.y / 2f;
            return room.bounds.center + side switch {
                DoorSide.Top => new Vector3(0, halfHeight),
                DoorSide.Bottom => new Vector3(0, -halfHeight),
                DoorSide.Left => new Vector3(-halfWidth, 0),
                DoorSide.Right => new Vector3(halfWidth, 0),
                _ => Vector3.zero,
            };
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