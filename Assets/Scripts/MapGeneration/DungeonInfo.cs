using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration {
    [Serializable]
    public class DungeonInfo : IEnumerable<TileInfo> {
        readonly Vector2Int _center;
        readonly int _width;
        readonly int _height;
        readonly int _border;
        readonly int _seed;

        readonly TileInfo[] _map;
        readonly List<RoomInfo> _rooms;

        public Vector2Int Center => _center;
        public int Width => _width;
        public int Height => _height;
        public int Border => _border;
        public int Seed => _seed;
        public IReadOnlyList<RoomInfo> Room => _rooms;
        public IReadOnlyList<TileInfo> Map => _map;

        public TileInfo this[int x, int y] {
            get {
                if (x < 0 || y < 0 || x >= _width || y >= _height) {
                    return new TileInfo() {
                        x = x,
                        y = y,
                        layer = TileLayer.Wall
                    };
                }
                return _map[y * _width + x];
            }
            set {
                if (x < 0 || y < 0 || x >= _width || y >= _height) return;
                _map[y * _width + x] = value;
            }
        }

        public DungeonInfo(Vector2Int center, int width, int height, int border, int seed) {
            _center = center;
            _width = width;
            _height = height;
            _border = border;
            _seed = seed;

            _map = new TileInfo[_width * _height];
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    _map[y * _width + x] = new TileInfo() {
                        layer = TileLayer.Floor,
                        x = x,
                        y = y,
                    };
                }
            }

            _rooms = new List<RoomInfo>();
        }

        public void AddRoom(RoomInfo room) {
            _rooms.Add(room);
        }

        public void SetMap(TileInfo[] map) {
            if (map.Length != _map.Length) return;
            for (int i = 0; i < _map.Length; i++) {
                _map[i] = map[i];
            }
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