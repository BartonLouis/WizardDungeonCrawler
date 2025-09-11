using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using Random = System.Random;
using Unity.Mathematics;

namespace Map.Generation.GenerationSteps {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Cave Generation Step", order = 12)]
    public class CaveGenerationStep : MapGenerationStep {
        [SerializeField] int _numIterations = 10;
        [SerializeField] float _adjacentWeight;
        [SerializeField] float _diagonalWeight;
        [SerializeField] float _weightThresholdForWall;
        [SerializeField] int _mapBorderThickness;

        NativeArray<TileType> _currentIteration;
        NativeArray<TileType> _nextIteration;
        NativeArray<Room> _rooms;

        public override void ApplyStep(Map map, Random random) {
            // Instantiate Native Arrays and populate with information
            _rooms = new NativeArray<Room>(map.rooms.Length, Allocator.TempJob);
            for (int i = 0; i < map.rooms.Length; i++) {
                _rooms[i] = map.rooms[i];
            }

            _currentIteration = new NativeArray<TileType>(map.tiles.Length, Allocator.TempJob);
            for (int i = 0; i < map.tiles.Length; i++) {
                _currentIteration[i] = map[i];
            }

            // Iterate x times to generate caves
            for (int i = 0; i < _numIterations; i++) {
                Iterate(map);
            }

            // Populate map with generated data
            for (int i = 0; i < map.tiles.Length; i++) {
                map[i] = _currentIteration[i];
            }
            _currentIteration.Dispose();
            _rooms.Dispose();
        }

        void Iterate(Map map) {
            var nextIter = new NativeArray<TileType>(_currentIteration.Length, Allocator.TempJob);
            var iterateJob = new AutomotaJob{
                borderThickness = _mapBorderThickness,
                width = map.Size.x,
                height = map.Size.y,
                adjacentWeight = _adjacentWeight,
                diagonalWeight = _diagonalWeight,
                requiredThreshold = _weightThresholdForWall,
                rooms = _rooms,
                currentState = _currentIteration,
                newState = nextIter,
            };
            var jobHandle = iterateJob.Schedule(_currentIteration.Length, 64);
            jobHandle.Complete();
            for (int i = 0; i < _currentIteration.Length; i++) {
                _currentIteration[i] = nextIter[i];
            }
            if(nextIter.IsCreated) {
                nextIter.Dispose();
            }
            map.changedFlag = true;
        }

        [BurstCompile]
        struct AutomotaJob : IJobParallelFor {
            public int iterationCount;
            public int borderThickness;
            public int width;
            public int height;

            public float adjacentWeight;
            public float diagonalWeight;
            public float requiredThreshold;
            [ReadOnly] public NativeArray<Room> rooms;
            [ReadOnly] public NativeArray<TileType> currentState;
            public NativeArray<TileType> newState;

            public void Execute(int index) {
                int xPos = index % width;
                int yPos = index / width;

                // Tile is along the border of the map, so ensure it remains a wall
                if (xPos < borderThickness 
                    || xPos >= width - borderThickness
                    || yPos < borderThickness 
                    || yPos >= height - borderThickness) {
                    newState[index] = TileType.Wall;
                    return;
                }

                // If tile is within the border of one of the rooms
                int worldXPos = xPos - width / 2;
                int worldYPos = height / 2 - yPos;
                foreach (Room room in rooms) {
                    var bounds = room.BorderBounds;
                    if (worldXPos >= bounds.minX 
                        && worldXPos < bounds.maxX 
                        && worldYPos >= bounds.minY 
                        && worldYPos < bounds.maxY) {
                        newState[index] = currentState[index];
                        return;
                    }
                }

                // Sum the weights of neighbouring tiles
                float weightSum = 0;
                for (int x = -1; x <= 1; x++) {
                    for (int y = -1; y <= 1; y++) {
                        int neighbourIndex = index + (y * width) + x;
                        if (neighbourIndex < 0 || neighbourIndex >= currentState.Length) {
                            newState[index] = TileType.Wall;
                            continue;
                        } else if (currentState[neighbourIndex] == TileType.Wall) {
                            weightSum += math.abs(x + y) % 2 == 0 ? adjacentWeight : diagonalWeight;
                        }
                    }
                }

                // If threshold is high enough, make this tile a wall, otherwise floor
                newState[index] = weightSum > requiredThreshold ? TileType.Wall : TileType.Floor;
            }
        }
    }
}
/*
 * using Unity.Burst;
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
 */