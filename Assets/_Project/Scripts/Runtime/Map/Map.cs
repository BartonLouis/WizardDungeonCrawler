using System;
using UnityEngine;

namespace Map {
    [Serializable]
    public class Map {
        Vector2Int _size;
        public Vector2Int Size {
            get => _size;
            set {
                _size = value;
                tiles = new TileType[value.x, value.y];
                changedFlag = true;
            }
        }
        public Room[] rooms;
        public Corridor[] corridors;
        public TileType[,] tiles;
        public bool changedFlag;

        public Map() {
            Size = new Vector2Int();
            rooms = new Room[0];
            corridors = new Corridor[0];
        }

        public void Draw() {
            foreach(var room in rooms)
                room.Draw();

            foreach(var corridor in corridors)
                corridor.Draw();
        }

        public (int, int) Get2DIndexes(int index) => (index % Size.x, index / Size.x);
        public int GetIndex(int x, int y) => y * Size.x + x;

        public TileType this[int index] {
            get {
                (int x, int y) = Get2DIndexes(index);
                return tiles[x, y];
            }
            set {
                (int x, int y) = Get2DIndexes(index);
                tiles[x, y] = value;
            }
        }

        public TileType this[int x, int y] {
            get => tiles[x, y];
            set => tiles[x, y] = value;
        }
    }

    public enum TileType {
        Floor = 0x0,
        Wall = 0x1
    }
}