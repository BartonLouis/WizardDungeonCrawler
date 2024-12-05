using Louis.Patterns.ServiceLocator;
using Louis.Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration {

    [Serializable]
    public class Dungeon : RegulatorSingleton<Dungeon>, IDungeonCreationService, IDungeonService {
        int _seed;
        Transform _transform;

        TileInfo[] _map;
        RoomInfo[] _rooms;
        public Transform Transform => _transform;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Border { get; private set; }
        public Vector2Int Center { get; private set; }
        public float Radius { get; private set; }
        public float FalloffRadius { get; private set; }
        public int Seed => _seed;
        public IReadOnlyList<TileInfo> Map => _map;
        public IReadOnlyList<RoomInfo> Rooms => _rooms;

        public TileInfo this[int x, int y] {
            get {
                x += Width / 2;
                y += Height / 2;
                if (x < 0 || y < 0 || x >= Width || y >= Height) {
                    return new TileInfo() {
                        x = x,
                        y = y,
                        layer = TileLayer.Wall
                    };
                }
                return _map[y * Width + x];
            }
            set {
                x += Width / 2;
                y += Height / 2;
                if (x < 0 || y < 0 || x >= Width || y >= Height) {
                    return;
                }
                _map[y * Width + x] = value;
            }
        }

        private void OnEnable() {
            ServiceLocator.Register<IDungeonService>(this);
            ServiceLocator.Register<IDungeonCreationService>(this);
        }

        private void OnDisable() {
            ServiceLocator.Deregister<IDungeonService>(this);
            ServiceLocator.Deregister<IDungeonCreationService>(this);
        }

        public void Init(int seed, Vector2Int center) {
            _transform = transform;
            ServiceLocator.Register<IDungeonService>(this);
            ServiceLocator.Register<IDungeonCreationService>(this);
            _seed = seed;

            Center = center;
            _map = new TileInfo[0];
            _rooms = new RoomInfo[0];
        }

        public void SetMap(TileInfo[] map, int width, int height, int radius, int falloffRadius, int border) {
            if (map.Length != width * height) {
                throw new ArgumentException($"Width ({width}) * Height ({height}) does not equal Map Size ({map.Length})");
            }
            Width = width;
            Height = height;
            Radius = radius;
            FalloffRadius = falloffRadius;
            _map = map;
            Border = border;
        }

        public void SetMap(TileInfo[] map) {
            if (map.Length != _map.Length)
                for (int i = 0; i < _map.Length; i++) {
                    _map[i] = map[i];
                }
        }

        public void SetRooms(RoomInfo[] rooms) {
            _rooms = rooms;
        }

        public IEnumerator<TileInfo> GetEnumerator() {
            foreach (TileInfo tile in _map) {
                yield return tile;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _map.GetEnumerator();
        }
    }
}