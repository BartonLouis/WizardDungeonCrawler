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
        [SerializeField] int _corridorSize;
        [Tooltip("This is the margin around all rooms which corridors must be outside to be considered safe")]
        [SerializeField] int _marginOfSafety = 3;
        DungeonInfo _dungeon;

        public override void Generate(DungeonInfo dungeon) {
            _dungeon = dungeon;
            Vector2[] roomCenters = _dungeon.Rooms.Select(r => (Vector2)r.bounds.center).ToArray();
            Delaunator delaunator = new Delaunator(roomCenters.ToPoints());
            List<CorridorInfo> corridors = new();
            delaunator.ForEachTriangleEdge((e) => {
                Vector2 p1 = e.P.ToVector2();
                Vector2 p2 = e.Q.ToVector2();

                RoomInfo start = _dungeon.Rooms.FirstOrDefault(r => r.bounds.center == (Vector3)p1);
                RoomInfo end = _dungeon.Rooms.FirstOrDefault(r => r.bounds.center == (Vector3)p2);

                CorridorInfo corridor = new CorridorInfo() {
                    start = start,
                    end = end,
                };

                bool intersectsAny = false;
                foreach (var room in dungeon.Rooms) {
                    if (room == start || room == end) {
                        continue;
                    }
                    if (room.bounds.IntersectedByLine((start.bounds.center, end.bounds.center), _marginOfSafety)) {
                        intersectsAny = true;
                        break;
                    }
                }
                if (!intersectsAny) {
                    corridors.Add(corridor);
                }
            });



            foreach (var corridor in corridors) {
                DrawCorridor(corridor);
            }
        }

        void DrawCorridor(CorridorInfo corridor) {
            Vector2[] startDoors = corridor.start.GenerateDoors();
            Vector2[] endDoors = corridor.end.GenerateDoors();
            Vector2 startDoor = startDoors[0];
            Vector2 endDoor = endDoors[0];
            float shortest = float.MaxValue;
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    float dist = Vector2.Distance(startDoors[i], endDoors[j]);
                    if (dist < shortest) {
                        startDoor = startDoors[i];
                        endDoor = endDoors[j];
                        shortest = dist;
                    }
                }
            }

            Bresenham(corridor.start.bounds.center, startDoor);
            Bresenham(startDoor, endDoor);
            Bresenham(endDoor, corridor.end.bounds.center);

        }

        void Bresenham(Vector2 start, Vector2 end) {
            // Bresenham algorithm to rasteurize line along corridor
            int x0 = (int)start.x;
            int y0 = (int)start.y;
            int x1 = (int)end.x;
            int y1 = (int)end.y;
            int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2;
            for (; ; ) {
                BrushOnPoint(new Vector2Int(x0, y0));

                if (x0 == x1 && y0 == y1) break;

                e2 = 2 * err;

                // horizontal step?
                if (e2 > dy) {
                    err += dy;
                    x0 += sx;
                }

                // vertical step?
                else if (e2 < dx) {
                    err += dx;
                    y0 += sy;
                }
            }
        }


        void BrushOnPoint(Vector2Int point) {
            int min = -Mathf.FloorToInt(_corridorSize / 2f);
            int max = Mathf.CeilToInt(_corridorSize / 2f);
            for (int x = min; x < max; x++) {
                for (int y = min; y < max; y++) {
                    Vector2Int newPoint = point + new Vector2Int(x, y);
                    _dungeon[newPoint.x, newPoint.y] = new TileInfo(_dungeon[newPoint.x, newPoint.y], TileLayer.Floor);
                }
            }
        }




        struct CorridorInfo {
            public RoomInfo start;
            public RoomInfo end;

            public readonly float Length => Vector3.Distance(start.bounds.center, end.bounds.center);
        }
    }
}