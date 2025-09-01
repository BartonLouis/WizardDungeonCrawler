using System;
using UnityEngine;

namespace Map {
    [Serializable]
    public struct Corridor {
        public int room1Index;
        public int room2Index;
        public Vector2Int[] positions;
        public int width;

        public float Length {
            get {
                float total = 0f;
                for (int i = 0; i < positions.Length - 1; i++) {
                    total += Vector2Int.Distance(positions[i], positions[i + 1]);
                }
                return total;
            }
        }

        public void Draw() { 
            for (int i = 0; i < positions.Length - 1; i++) {
                Debug.DrawLine((Vector2)positions[i], (Vector2)positions[i + 1], Color.magenta);
            }
        }
    }
}