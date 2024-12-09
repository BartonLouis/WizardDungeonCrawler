using Louis.Patterns.ServiceLocator;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DungeonGeneration {
    public interface IDungeonGeneratorService : IService {
        public void Generate(int seed);
        public void Generate();
    }

    public interface IDungeonCreationService : IService, IEnumerable<TileInfo> {
        public void SetMap(TileInfo[] map, int width, int height, int radius, int falloffRadius, int border);
        public void SetMap(TileInfo[] map);
        public void SetRooms(RoomInfo[] rooms);
        public TileInfo this[int x, int y] { get; set; }
        public Transform Transform { get; }
    }

    public interface IDungeonService : IService {
        public int Width { get; }
        public int Height { get; }
        public int Border { get; }
        public Vector2Int Center { get; }
        public float Radius { get; }
        public float FalloffRadius { get; }
        public int Seed { get; }

        public IReadOnlyList<TileInfo> Map { get; }
        public IReadOnlyList<RoomInfo> Rooms { get; }
        public TileInfo this[int x, int y] { get; }
        public void ShowRoomLabels(Transform parent, TextMeshProUGUI prefab);
    }
}