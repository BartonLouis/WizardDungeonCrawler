using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DungeonGeneration {

    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Cave Generation Step")]
    public class CaveGenerationStep : AbstractGenerationStep {
        [Header("Settings")]
        [SerializeField] int _numIterations;
        [SerializeField] float _adjacentWeight;
        [SerializeField] float _diagonalWeight;
        [SerializeField] float _weightThresholForWall;
        [SerializeField] bool _generateWithinRoomBounds;

        NativeArray<TileInfo> currentIteration;
        NativeArray<TileInfo> nextIteration;
        NativeArray<RoomInfo> rooms;

        Dungeon _dungeon;

        public override void Generate(Dungeon dungeon) {
            _dungeon = dungeon;
            currentIteration = new NativeArray<TileInfo>(_dungeon.Map.Count, Allocator.TempJob);
            rooms = new NativeArray<RoomInfo>(_dungeon.Rooms.Count, Allocator.TempJob);
            for (int i = 0; i < _dungeon.Map.Count; i++) {
                currentIteration[i] = _dungeon.Map[i];
            }
            for (int i = 0; i < _dungeon.Rooms.Count; i++) {
                rooms[i] = _dungeon.Rooms[i];
            }

            for (int i = 0; i < _numIterations; i++) {
                Iterate();
            }
            for (int i = 0; i < dungeon.Map.Count; i++) {
                TileInfo tile = currentIteration[i];
                dungeon[tile.x, tile.y] = tile;
            }
            currentIteration.Dispose();
            rooms.Dispose();
        }

        void Iterate() {
            nextIteration = new NativeArray<TileInfo>(currentIteration.Length, Allocator.TempJob);
            var iterateJob = new UpdateAutomotaJob {
                outerThickness = _dungeon.Border,
                width = _dungeon.Width,
                height = _dungeon.Height,
                generateWithinRoomBounds = _generateWithinRoomBounds,
                adjacentWeight = _adjacentWeight,
                diagonalWeight = _diagonalWeight,
                requiredThreshold = _weightThresholForWall,
                currentState = currentIteration,
                nextState = nextIteration,
                rooms = rooms
            };
            JobHandle jobHandle = iterateJob.Schedule(currentIteration.Length, 64);
            jobHandle.Complete();
            for (int i = 0; i < currentIteration.Length; i++) {
                currentIteration[i] = nextIteration[i];
            }
            if (nextIteration.IsCreated) {
                nextIteration.Dispose();
            }
        }
    }

    [BurstCompile]
    public struct UpdateAutomotaJob : IJobParallelFor {
        public int outerThickness;
        public int width;
        public int height;
        public bool generateWithinRoomBounds;

        public float adjacentWeight;
        public float diagonalWeight;
        public float requiredThreshold;
        [ReadOnly] public NativeArray<TileInfo> currentState;
        [ReadOnly] public NativeArray<RoomInfo> rooms;
        public NativeArray<TileInfo> nextState;

        public void Execute(int index) {
            TileInfo tile = currentState[index];
            // If tile is on the border, ignore
            if (tile.x <= -width / 2 + outerThickness 
                || tile.x >= width / 2 - outerThickness - 1 
                || tile.y <= -width / 2 + outerThickness 
                || tile.y >= height / 2 - outerThickness - 1) {
                tile.layer = TileLayer.Wall;
                nextState[index] = tile;
                return;
            }

            // If tile is within the border of one of the rooms
            if (!generateWithinRoomBounds) {
                foreach (RoomInfo room in rooms) {
                    if (tile.x >= room.bounds.min.x + room.margin + room.border - 1
                        && tile.x <= room.bounds.max.x - room.margin - room.border + 1
                        && tile.y >= room.bounds.min.y + room.margin + room.border - 1
                        && tile.y <= room.bounds.max.y - room.margin - room.border + 1) {
                        nextState[index] = tile;
                        return;
                    }
                }
            }

            // Sum weights of neighouring tiles
            float weightSum = 0;
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    int neighbourIndex = index + (y * width) + x;
                    if (neighbourIndex < 0 || neighbourIndex >= currentState.Length) {
                        tile.layer = TileLayer.Wall;
                        nextState[index] = tile;
                        return;
                    } else if (currentState[neighbourIndex].layer == TileLayer.Wall) {
                        weightSum += math.abs(x + y) % 2 == 0 ? adjacentWeight : diagonalWeight;
                    }
                }
            }

            tile.layer = weightSum > requiredThreshold ? TileLayer.Wall : TileLayer.Floor;
            nextState[index] = tile;
        }
    }
}