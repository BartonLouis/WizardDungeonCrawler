using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Kruskals MST Generation Step", order = 6)]
    public class KruskalsMSTGeneratorStep : MapGenerationStep {
        [Header("Settings")]
        [Range(0f, 1f), SerializeField] float _chanceToAddExtra;


        public override void ApplyStep(Map map, Random random) {
            List<Corridor> sortedCorridors = map.corridors.OrderBy(c => c.Length).ToList();
            List<Corridor> unchosen = new();
            DisjointSet dsu = new(map.rooms.Length);
            List<Corridor> mst = new();

            for (int i = 0; i < sortedCorridors.Count; i++) {
                var corridor = sortedCorridors[i];
                if (!dsu.Union(corridor.room1Index, corridor.room2Index)) {
                    unchosen.Add(corridor);
                    continue;
                }

                mst.Add(corridor);

                if (mst.Count == map.rooms.Length - 1) {
                    unchosen.AddRange(sortedCorridors.Skip(i+1));
                    break;
                }
            }

            foreach(var corridor in unchosen) {
                if (random.NextDouble() < _chanceToAddExtra) mst.Add(corridor);
            }

            map.corridors = mst.ToArray();
        }

        class DisjointSet {
            readonly int[] parent;
            readonly int[] rank;

            public DisjointSet(int size) {
                parent = new int[size];
                rank = new int[size];
                for (int i = 0; i < size; i++) {
                    parent[i] = i;
                    rank[i] = 0;
                }
            }

            public int Find(int x) {
                if(parent[x] != x) {
                    parent[x] = Find(parent[x]); // Recursively iterate through parents until we find a node which is its own parent
                }
                return parent[x];
            }

            public bool Union(int x, int y) {
                int rootX = Find(x);
                int rootY = Find(y);

                if(rootX == rootY) return false; // Already connected

                // Union by rank
                if(rank[rootX] < rank[rootY]) {
                    parent[rootX] = rootY;
                } else if(rank[rootX] > rank[rootY]) {
                    parent[rootY] = rootX;
                } else {
                    parent[rootY] = rootX;
                    rank[rootX]++;
                }
                return true;
            }
        }
    }
}