using DungeonGeneration;
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
    public static class DungeonInfoExtensions {

        public static bool IsWithinBorder(this DungeonInfo dungeon, int x, int y) {
            return (x >= dungeon.Border && x < dungeon.Width - dungeon.Border && y >= dungeon.Border && y < dungeon.Height - dungeon.Border);
        }

        public static IEnumerable<Vector2Int> IterateThroughBounds(this BoundsInt bounds) {
            for (int x = bounds.min.x; x < bounds.max.x; x++) {
                for (int y = bounds.min.y; y < bounds.max.y; y++) {
                    yield return new Vector2Int(x, y);
                }
            }
        }
    }
}