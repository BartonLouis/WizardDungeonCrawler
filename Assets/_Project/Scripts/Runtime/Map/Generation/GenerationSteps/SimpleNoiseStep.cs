using System;
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

        public override void ApplyStep(Map map, Random random) {
            int width = map.Size.x;
            int height = map.Size.y;
            NativeArray<TileType> tiles = new NativeArray<TileType>(width * height, Allocator.TempJob);
            var job = new SimpleNoiseTileJob(){
                seed = (uint)random.Next(),
                wallChance = _wallChance,
                tiles = tiles
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


        #region // Job for randomly generating a wall or floor tile
        struct SimpleNoiseTileJob : IJobParallelFor {
            public uint seed;
            public float wallChance;
            public NativeArray<TileType> tiles;

            public void Execute(int index) {
                uint localSeed = 1 +  seed * (uint)index * (uint)index;
                Unity.Mathematics.Random rnd = new(localSeed);
                float value = rnd.NextFloat();
                tiles[index] = value < wallChance ? TileType.Wall : TileType.Floor;
            }
        }
        #endregion
    }
}
