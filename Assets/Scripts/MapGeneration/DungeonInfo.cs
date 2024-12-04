using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration {
    [Serializable]
    public class DungeonInfo : IEnumerable<TileInfo> {
        readonly int _seed;

        TileInfo[] _map;
        RoomInfo[] _rooms;

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

        public DungeonInfo(int seed, Vector2Int center) {
            _seed = seed;

            Center = center;
            _map = new TileInfo[0];
            _rooms = new RoomInfo[0];
        }

        public void SetMap(TileInfo[] map, int width, int height, int radius, int falloffRadius) {
            if (map.Length != width * height) {
                throw new ArgumentException($"Width ({width}) * Height ({height}) does not equal Map Size ({map.Length})");
            }
            Width = width; 
            Height = height;
            Radius = radius;
            FalloffRadius = falloffRadius;
            _map = map;
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
            foreach(TileInfo tile in _map) {
                yield return tile;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _map.GetEnumerator();
        }
    }

    [Serializable]
    public struct RoomInfo {
        public BoundsInt bounds;
        public int margin;
        public int border;

        public RoomInfo(RoomInfo old, Vector3Int position) {
            margin = old.margin;
            border = old.border;
            bounds = new BoundsInt(position, old.bounds.size);
        }
    }

    [Serializable]
    public struct TileInfo {
        public TileLayer layer;
        public int x;
        public int y;

        public TileInfo(TileInfo old, TileLayer layer) {
            x = old.x;
            y = old.y;
            this.layer = layer;
        }

        public override readonly int GetHashCode() => HashCode.Combine(x, y);
        public override readonly bool Equals(object obj) => obj is TileInfo other && other.GetHashCode() == GetHashCode();
    }


    public enum TileLayer {
        None,
        Floor,
        Wall
    }
}