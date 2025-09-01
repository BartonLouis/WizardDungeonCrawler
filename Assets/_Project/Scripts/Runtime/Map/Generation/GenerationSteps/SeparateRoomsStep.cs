using UnityEngine;
using Random = System.Random;

namespace Map.Generation.GenerationSteps {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Separate Rooms Step", order = 2)]
    internal class SeparateRoomsStep : MapGenerationStep {
        [Header("Settings")]
        [SerializeField] int _maxIterations;
        [SerializeField] float _maxStepSize;

        public override void ApplyStep(Map map, Random random) {
            for (int i = 0; i < _maxIterations; i++) {
                if(!SeparateIteration(map)) break;
            }
        }

        bool SeparateIteration(Map map) {
            Vector2[] forces = new Vector2[map.rooms.Length];
            bool moved = false;
            for(int i = 0; i < map.rooms.Length; i++) {
                for(int j = i + 1; j < map.rooms.Length; j++) {

                    Vector2 f = SeparationForce(map.rooms[i], map.rooms[j]);
                    if(f != Vector2Int.zero) moved = true;
                    forces[i] += f;
                    forces[j] -= f;
                }
            }
            for(int i = 0; i < map.rooms.Length; i++) {
                Vector2Int force = Vector2Int.RoundToInt(forces[i]);
                force = new Vector2Int(
                    (int)Mathf.Clamp(force.x, -_maxStepSize, _maxStepSize),
                    (int)Mathf.Clamp(force.y, -_maxStepSize, _maxStepSize)
                    );
                map.rooms[i].position += force;
            }
            return moved;
        }

        Vector2 SeparationForce(Room a, Room b) {
            Vector2 delta = (b.Center - a.Center);

            // Distances between centers
            float dx = Mathf.Abs(delta.x);
            float dy = Mathf.Abs(delta.y);

            // Minimum allowed distances
            float minDx = Room.RequiredGapX(a, b);
            float minDy = Room.RequiredGapY(a, b);

            Vector2 force = Vector2.zero;
            // Only apply force if overlapping
            if(dx < minDx && dy < minDy) {
                float overlapX = minDx - dx;
                float overlapY = minDy - dy;


                if(overlapX < overlapY)
                    force = new Vector2(Mathf.Sign(delta.x) * -overlapX, 0f);
                else
                    force = new Vector2(0f, Mathf.Sign(delta.y) * -overlapY);
            }
            return force;
        }
    }
}