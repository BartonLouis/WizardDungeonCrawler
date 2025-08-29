using andywiecko.BurstTriangulator;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Delaunay Triangulation Corridor Generation Step", order = 4)]
    public class DelaunayCorridorGenerationStep : MapGenerationStep {
        public override void ApplyStep(Map map, Random random) {
            var roomPositions = new NativeArray<Vector2>(map.rooms.Length, Allocator.Persistent);
            for(int i = 0; i < map.rooms.Length; i++) {
                roomPositions[i] = map.rooms[i].Center;
            }

            var triangulator = new Triangulator<Vector2>(Allocator.Persistent) {
                Input = { Positions = roomPositions }
            };
            triangulator.Run();

            var positions = triangulator.Output.Positions;
            var triangles = triangulator.Output.Triangles;  // Buffer which takes the form [t0_a, t0_b, t0_c, t1_a, t1_b, t1_c ]

            var edgesSet = new HashSet<(int, int)>();
            for(int i = 0; i < triangles.Length; i += 3) {
                int a = triangles[i];
                int b = triangles[i+1];
                int c = triangles[i+2];

                void TryAddEdge(int v1, int v2) {
                    if(v1 == v2) return;
                    var edge = (v1, v2);
                    edgesSet.Add(edge);
                }

                TryAddEdge(a, b);
                TryAddEdge(b, c);
                TryAddEdge(c, a);
            }

            List<Corridor> corridors = new();
            foreach(var (id1, id2) in edgesSet) {
                var p1 = map.rooms[id1].Center;
                var p2 = map.rooms[id2].Center;
                corridors.Add(new() {
                    room1Index = id1,
                    room2Index = id2,
                    positions = new Vector2Int[]{
                        Vector2Int.RoundToInt(p1),
                        Vector2Int.RoundToInt(p2)
                    },
                    width = 1
                });
            }
            roomPositions.Dispose();
            triangulator.Dispose();
            map.corridors = corridors.ToArray();
        }
    }
}