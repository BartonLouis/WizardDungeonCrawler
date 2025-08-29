using System;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Corridor Door Alignment Step", order = 5)]
    public class CorridorDoorAlignmentStep : MapGenerationStep {

        int _currentCorridorIndex;

        public override void Init() {
            _currentCorridorIndex = 0;
        }

        public override bool ApplyStep(Map map, Random random) {
            if(_currentCorridorIndex >= map.corridors.Length) return true;
            Corridor corridor = map.corridors[_currentCorridorIndex];
            // Do stuff here
            Room r1 = map.rooms[corridor.room1Index];
            Room r2 = map.rooms[corridor.room2Index];

            float smallestSqrDistance = float.MaxValue;
            Vector2Int room1Door = Vector2Int.zero;
            Vector2Int room2Door = Vector2Int.zero;
            foreach(var door1 in r1.Doors) {
                foreach(var door2 in r2.Doors) {
                    float sqrDistance = (door1 - door2).sqrMagnitude;
                    if (sqrDistance < smallestSqrDistance) {
                        smallestSqrDistance = sqrDistance;
                        room1Door = door1;
                        room2Door = door2;
                    }
                }
            }
            corridor = new Corridor() {
                room1Index = corridor.room1Index,
                room2Index = corridor.room2Index,
                positions = new Vector2Int[] { room1Door, room2Door },
                width = corridor.width
            };
            map.corridors[_currentCorridorIndex] = corridor;

            _currentCorridorIndex++;
            return _currentCorridorIndex >= map.corridors.Length;
        }
    }
}