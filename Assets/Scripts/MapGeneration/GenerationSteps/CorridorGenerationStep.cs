using DelaunatorSharp;
using DelaunatorSharp.Unity.Extensions;
using Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace DungeonGeneration {
    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Corridor Generation Step")]
    public class CorridorGenerationStep : AbstractGenerationStep {
        [Header("Settings")]
        [Tooltip("This is the margin around all rooms which corridors must be outside to be considered safe")]
        [SerializeField] int _marginOfSafety = 3;
        Dungeon _dungeon;

        public override void Generate(Dungeon dungeon) {
            _dungeon = dungeon;
            Vector2[] roomCenters = _dungeon.Rooms.Select(r => (Vector2)r.bounds.center).ToArray();
            Delaunator delaunator = new Delaunator(roomCenters.ToPoints());
            List<CorridorInfo> corridors = new();
            delaunator.ForEachTriangleEdge((e) => {
                Vector2 p1 = e.P.ToVector2();
                Vector2 p2 = e.Q.ToVector2();

                int startRoomIndex = 0;
                int endRoomIndex = 0;
                for (int i = 0; i < _dungeon.Rooms.Count; i++) {
                    if (_dungeon.Rooms[i].bounds.center == (Vector3)p1) startRoomIndex = i;
                    if (_dungeon.Rooms[i].bounds.center == (Vector3)p2) endRoomIndex = i;
                }

                RoomInfo startRoom = _dungeon.Rooms[startRoomIndex];
                RoomInfo endRoom = _dungeon.Rooms[endRoomIndex];

                CorridorInfo corridor = CreateCorridor(startRoomIndex, endRoomIndex);
                if (corridor == default) return;

                bool intersectsAny = false;
                foreach (var room in dungeon.Rooms) {
                    if (room == startRoom || room == endRoom) {
                        continue;
                    }

                    if (room.bounds.IntersectedByLine((corridor.startDoor, corridor.endDoor), _marginOfSafety)) {
                        intersectsAny = true;
                        break;
                    }
                }
                if (!intersectsAny) {
                    corridors.Add(corridor);
                }
            });

            _dungeon.SetCorridors(corridors.ToArray());
        }

        CorridorInfo CreateCorridor(int startRoomIndex, int endRoomIndex) {
            RoomInfo startRoom = _dungeon.Rooms[startRoomIndex];
            RoomInfo endRoom = _dungeon.Rooms[endRoomIndex];
            Side startSide = Side.Top;
            Side endSide = Side.Top;
            Vector2 startDoor;
            Vector2 endDoor;
            float shortest = float.MaxValue;
            foreach(Side s1 in EnumUtils.GetValues<Side>()) {
                foreach(Side s2 in EnumUtils.GetValues<Side>()) {
                    Vector2 d1 = startRoom.GenerateDoor(s1);
                    Vector2 d2 = endRoom.GenerateDoor(s2);
                    float dist = Vector2.Distance(d1, d2);
                    if (dist < shortest) {
                        shortest = dist;
                        startSide = s1;
                        endSide = s2;
                        startDoor = d1;
                        endDoor = d2;
                    }
                }
            }
            return new(startRoomIndex, endRoomIndex, startSide, endSide, _dungeon);
        }
    }
}