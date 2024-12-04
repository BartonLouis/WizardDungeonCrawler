using System.Collections.Generic;
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
    }
}