using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation.GenerationSteps {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Cull Overlapping Corridors Step", order = 5)]
    internal class CullOverlappingCorridorsStep : MapGenerationStep {
        public override void ApplyStep(Map map, Random random) {
            map.corridors = map.corridors.Where(c => !OverlapsRoom(c, map)).ToArray();
        }

        static bool OverlapsRoom(Corridor corridor, Map map) {
            for(int i = 0; i < map.rooms.Length; i++) {
                if(i == corridor.room1Index || i == corridor.room2Index) continue;
                if(CorridorOverlapsBounds(corridor, map.rooms[i].SafeBounds)) return true;
            }
            return false;
        }

        static bool CorridorOverlapsBounds(Corridor corridor, BoundsInt2D bounds) {
            for(int i = 0; i < corridor.positions.Length - 1; i++) {
                if(SegmentIntersectsBounds(corridor.positions[i], corridor.positions[i + 1], bounds))
                    return true;
            }
            return false;
        }

        static bool SegmentIntersectsBounds(Vector2Int a, Vector2Int b, BoundsInt2D bounds) {
            // quick reject
            if(a.x < bounds.minX && b.x < bounds.minX) return false;
            if(a.x > bounds.maxX && b.x > bounds.maxX) return false;
            if(a.y < bounds.minY && b.y < bounds.minY) return false;
            if(a.y > bounds.maxY && b.y > bounds.maxY) return false;

            // if either endpoint is inside
            if(PointInBounds(a, bounds) || PointInBounds(b, bounds))
                return true;

            // rectangle corners
            Vector2[] rect = {
            new Vector2(bounds.minX, bounds.minY),
            new Vector2(bounds.maxX, bounds.minY),
            new Vector2(bounds.maxX, bounds.maxY),
            new Vector2(bounds.minX, bounds.maxY)
        };

            // check against each edge of the rectangle
            for(int i = 0; i < 4; i++) {
                if(SegmentsIntersect(a, b, rect[i], rect[(i + 1) % 4]))
                    return true;
            }

            return false;
        }

        static bool PointInBounds(Vector2Int p, BoundsInt2D b) =>
            p.x >= b.minX && p.x <= b.maxX && p.y >= b.minY && p.y <= b.maxY;

        static bool SegmentsIntersect(Vector2Int p1, Vector2Int p2, Vector2 q1, Vector2 q2) {
            Vector2 a = p1, b = p2;

            return LinesIntersect(a, b, q1, q2);
        }

        // Uses Unity Vector2 cross product trick
        static bool LinesIntersect(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2) {
            return (CCW(p1, q1, q2) != CCW(p2, q1, q2)) &&
                   (CCW(p1, p2, q1) != CCW(p1, p2, q2));
        }

        static bool CCW(Vector2 a, Vector2 b, Vector2 c) {
            return (c.y - a.y) * (b.x - a.x) > (b.y - a.y) * (c.x - a.x);
        }
    }
}