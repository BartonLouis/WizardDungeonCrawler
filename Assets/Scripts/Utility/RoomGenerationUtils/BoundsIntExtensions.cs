using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils {
    public static class BoundsIntExtensions {
        public static bool Intersects(this BoundsInt a, BoundsInt b) {
            return (
                a.min.x < b.max.x &&
                a.max.x > b.min.x &&
                a.min.y < b.max.y &&
                a.max.y > b.min.y
                );
        }

        public static bool AnyIntersect(IReadOnlyList<BoundsInt> bounds) {
            for (int i = 0; i < bounds.Count - 1; i++) {
                for (int j = i + 1; j < bounds.Count; j++) {
                    if (bounds[i].Intersects(bounds[j])) return true;
                }
            }
            return false;
        }

        public static bool IntersectedByLine(this BoundsInt bounds, (Vector2, Vector2) line, int marginOfSafety) {
            BoundsInt newBounds = new BoundsInt(bounds.position - new Vector3Int(marginOfSafety, marginOfSafety), bounds.size + new Vector3Int(2 * marginOfSafety, 2 * marginOfSafety));
            return new (Vector2, Vector2)[4] {
                (new Vector2(newBounds.min.x, newBounds.min.y), new Vector2(newBounds.min.x, newBounds.max.y)),
                (new Vector2(newBounds.min.x, newBounds.min.y), new Vector2(newBounds.max.x, newBounds.min.y)),
                (new Vector2(newBounds.min.x, newBounds.max.y), new Vector2(newBounds.max.x, newBounds.max.y)),
                (new Vector2(newBounds.max.x, newBounds.min.y), new Vector2(newBounds.max.x, newBounds.max.y)),
            }.Any(s => line.Intersects(s));
        }

        public static bool Intersects(this (Vector2, Vector2) l1, (Vector2, Vector2) l2) {
            float dx0 = l1.Item2.x - l1.Item1.x;
            float dx1 = l2.Item2.x - l2.Item1.x;
            float dy0 = l1.Item2.y - l1.Item1.y;
            float dy1 = l2.Item2.y - l2.Item1.y;
            float p0 = dy1 * (l2.Item2.x - l1.Item1.x) - dx1 * (l2.Item2.y - l1.Item1.y);
            float p1 = dy1 * (l2.Item2.x - l1.Item2.x) - dx1 * (l2.Item2.y - l1.Item2.y);
            float p2 = dy0 * (l1.Item2.x - l2.Item1.x) - dx0 * (l1.Item2.y - l2.Item1.y);
            float p3 = dy0 * (l1.Item2.x - l2.Item2.x) - dx0 * (l1.Item2.y - l2.Item2.y);
            bool intersects = (p0 * p1 <= 0) & (p2 * p3 <= 0);
            return intersects;
        }
    }
}