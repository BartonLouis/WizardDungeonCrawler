using UnityEngine;
using Random = System.Random;

namespace Map.Generation {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Corridor Door Alignment Step", order = 6)]
    public class CorridorDoorAlignmentStep : MapGenerationStep {

        public override void ApplyStep(Map map, Random random) {
            for (int i = 0; i < map.corridors.Length; i++) {
                map.corridors[i] = AlignCorridor(map, i);
            }
        }

        Corridor AlignCorridor(Map map, int corridorIndex) {
            Corridor corridor = map.corridors[corridorIndex];
            // Do stuff here
            Room r1 = map.rooms[corridor.room1Index];
            Room r2 = map.rooms[corridor.room2Index];

            float smallestSqrDistance = float.MaxValue;
            Vector2Int room1Door = Vector2Int.zero;
            Vector2Int room2Door = Vector2Int.zero;
            foreach(var door1 in r1.Doors) {
                foreach(var door2 in r2.Doors) {
                    float sqrDistance = (door1 - door2).sqrMagnitude;
                    if(sqrDistance < smallestSqrDistance) {
                        smallestSqrDistance = sqrDistance;
                        room1Door = door1;
                        room2Door = door2;
                    }
                }
            }

            var positions = new Vector2Int[4]{
                room1Door,
                GetDoorExit(room1Door, r1),
                GetDoorExit(room2Door, r2),
                room2Door
            };
            return new Corridor() {
                room1Index = corridor.room1Index,
                room2Index = corridor.room2Index,
                positions = positions,
                width = corridor.width
            };
        }

        Vector2Int GetDoorExit(Vector2Int door, Room room) {
            Vector2Int direction = Vector2Int.RoundToInt(door - room.Center);
            direction = direction / Mathf.Max(Mathf.RoundToInt(direction.magnitude), 1);
            return door + direction * (room.border + room.margin);
        }
    }
}