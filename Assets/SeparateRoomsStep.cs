using System.Linq;
using UnityEngine;

namespace MapGeneration {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Separate Rooms Step", order = 2)]
    public class SeparateRoomsStep : MapGenerationStep {
        [Header("Settings")]
        [SerializeField] int _maxIterations;
        [Range(0f, 1f), SerializeField] float _pushAmount = .4f;

        public override Map ApplyStep(Map map) {
            for(int iter = 0; iter < _maxIterations; iter++) {
                // Returns true if some movement occured, so if it returns false, nothing overlapped, so we're done
                if(!SeparateIteration(map)) break;
            }

            return map;
        }


        bool SeparateIteration(Map map) {
            bool moved = false;
            for(int i = 0; i < map.rooms.Count(); i++) {
                for(int j = 0; j < map.rooms.Count(); j++) {
                    if(SeparatePair(ref map.rooms[i], ref map.rooms[j], _pushAmount)) moved = true;
                }
            }
            return moved;
        }

        static bool SeparatePair(ref Room r1, ref Room r2, float pushAmountCoefficient) {
            RectInt b1 = r1.GetBounds();
            RectInt b2 = r2.GetBounds();

            int requiredGap = Mathf.Max(r1.margin, r2.margin);

            // Expand both bounds by the margin
            RectInt expanded1 = ExpandRect(b1, requiredGap);
            RectInt expanded2 = ExpandRect(b2, requiredGap);

            if(!expanded1.Overlaps(expanded2))
                return false;

            // Calculate the overlap vector
            int overlapX = Mathf.Min(expanded1.xMax, expanded2.xMax) - Mathf.Max(expanded1.xMin, expanded2.xMin);
            int overlapY = Mathf.Min(expanded1.yMax, expanded2.yMax) - Mathf.Max(expanded1.yMin, expanded2.yMin);

            Vector2 pushDir;
            int pushAmount;

            if (overlapX < overlapY) {
                // Push along X axis
                pushDir = new Vector2(Mathf.Sign(r1.position.x - r2.position.x), 0);
                pushAmount = overlapX;
            } else {
                // Push along Y axis
                pushDir = new Vector2(0, Mathf.Sign(r1.position.y - r2.position.y));
                pushAmount = overlapY;
            }

            // Instead of full MTV, push only a fraction (but at least 1)
            int step = Mathf.Max(1, Mathf.RoundToInt(pushAmount * pushAmountCoefficient));

            // Split push between rooms
            Vector2Int offset = Vector2Int.RoundToInt(pushDir * step);
            r1.position += offset;
            r2.position -= offset;

            return true;
        }

        static RectInt ExpandRect(RectInt rect, int amount) {
            return new RectInt(
                rect.xMin - amount,
                rect.yMin - amount,
                rect.width + amount * 2,
                rect.height + amount * 2
                );
        }
    }
}