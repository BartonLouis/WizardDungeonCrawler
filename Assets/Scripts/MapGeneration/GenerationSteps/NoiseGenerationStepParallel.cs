using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

namespace DungeonGeneration {

    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Noise Generation Parallel Step")]
    public class NoiseGenerationStepParallel : AbstractGenerationStep {

        [Header("Settings")]
        [Range(0f, 1f)]
        [SerializeField] float _wallDensity;


        public override void Generate(Dungeon dungeon) {
            Random random = new(dungeon.Seed);
            NativeArray<TileInfo> tiles = new NativeArray<TileInfo>(dungeon.Map.Count, Allocator.TempJob);
            NativeArray<uint> seeds = new NativeArray<uint>(dungeon.Map.Count, Allocator.TempJob);
            for (int i = 0; i < dungeon.Map.Count; i++) {
                tiles[i] = dungeon.Map[i];
                seeds[i] = (uint)random.Next();
            }
            var iterateJob = new CalculateNoiseJob {
                wallDensity = _wallDensity,
                innerRadius = dungeon.Radius,
                outerRadius = dungeon.FalloffRadius,
                map = tiles,
                seeds = seeds
            };
            JobHandle jobHandle = iterateJob.Schedule(tiles.Length, 128);
            jobHandle.Complete();
            foreach (var tile in tiles) {
                dungeon[tile.x, tile.y] = tile;
            }
            tiles.Dispose();
            seeds.Dispose();
        }
    }

    [BurstCompile]
    public struct CalculateNoiseJob : IJobParallelFor {
        public float wallDensity;
        public float innerRadius;
        public float outerRadius;

        public NativeArray<TileInfo> map;
        public NativeArray<uint> seeds;

        public void Execute(int index) {
            var rnd = new Unity.Mathematics.Random(seeds[index]);
            TileInfo tile = map[index];
            float dist = new Vector2(tile.x, tile.y).magnitude;
            if (dist < innerRadius) {
                tile.layer = rnd.NextFloat() < wallDensity ? TileLayer.Wall : TileLayer.Floor;
            } else if (dist < innerRadius + innerRadius) {
                float lerpedThreshold = math.lerp(wallDensity, 1, (dist - innerRadius) / outerRadius);
                tile.layer = rnd.NextFloat() < lerpedThreshold ? TileLayer.Wall : TileLayer.Floor;
            } else {
                tile.layer = TileLayer.Wall;
            }
            map[index] = tile;
        }
    }
}