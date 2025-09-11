using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation.GenerationSteps {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Simple Noise Step", order = 9)]
    public class SimpleNoiseStep : MapGenerationStep {
        [Range(0f, 1f), SerializeField] float _wallChance;
        [SerializeField] int _mapBorder = 5;
        [SerializeField, Range(0f, 1f)] float _fallOffStartPoint = .5f;
        [SerializeField, Range(0f, 1f)] float _fallOffRange = .5f;

        public override void ApplyStep(Map map, Random random) {
            int width = map.Size.x;
            int height = map.Size.y;

            NativeArray<TileType> tiles = new NativeArray<TileType>(width * height, Allocator.TempJob);
            var job = new SimpleNoiseTileJob(){
                seed = (uint)random.Next(),
                wallChance = _wallChance,
                tiles = tiles,
                mapRadius = Mathf.Max(width / 2, height / 2),
                width = width,
                height = height,
                fallOffStart = _fallOffStartPoint,
                fallOffRange = _fallOffRange
            };
            var jobHandle = job.Schedule(tiles.Length, 64);
            jobHandle.Complete();


            for(int x = _mapBorder; x < width - _mapBorder; x++) {
                for(int y = _mapBorder; y < height - _mapBorder; y++) {
                    map[x, y] = job.tiles[map.GetIndex(x, y)];
                }
            }
            map.changedFlag = true;

            tiles.Dispose();
        }

        private void OnValidate() {
            if (_fallOffStartPoint + _fallOffRange > 1) {
                _fallOffRange = 1 - _fallOffStartPoint;
            }
        }


        #region // Job for randomly generating a wall or floor tile
        [BurstCompile]
        struct SimpleNoiseTileJob : IJobParallelFor {
            public int width;
            public int height;
            public float mapRadius;
            public float fallOffStart;
            public float fallOffRange;


            public uint seed;
            [WriteOnly] public NativeArray<TileType> tiles;
            public float wallChance;

            public void Execute(int index) {
                // Calculate distance from center
                int x = (index % width) - width / 2;    // Converts x into world space position
                int y = height / 2 - (index / width);   // Converts y into world space position

                float distance = math.sqrt(x * x + y * y);
                float normDistance = distance / mapRadius;

                float normDistanceIntoFalloffRange = math.max(normDistance - fallOffStart, 0);
                float chance = math.lerp(wallChance, 1, normDistanceIntoFalloffRange / fallOffRange);




                // Generate Random number
                uint localSeed = 1 +  seed * (uint)index * (uint)index;
                Unity.Mathematics.Random rnd = new(localSeed);
                float value = rnd.NextFloat();


                tiles[index] = value < chance ? TileType.Wall : TileType.Floor;
            }
        }
        #endregion
    }
}
