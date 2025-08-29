using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Map {
    [Serializable]
    public class Map {
        public Vector2Int size;
        public Room[] rooms;
        public Corridor[] corridors;

        public Map() { 
            size = new Vector2Int();
            rooms = new Room[0];
            corridors = new Corridor[0];
        }

        public void Draw() {
            foreach(var room in rooms)
                room.Draw();

            foreach(var corridor in corridors)
                corridor.Draw();
        }
    }

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