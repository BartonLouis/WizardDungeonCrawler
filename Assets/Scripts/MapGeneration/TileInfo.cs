﻿using System;

namespace DungeonGeneration {
    [Serializable]
    public struct TileInfo {
        public int x;
        public int y;
        public TileLayer layer;

        public TileInfo(TileInfo old, TileLayer layer) {
            x = old.x;
            y = old.y;
            this.layer = layer;
        }

        public void SetLayer(TileLayer layer) {
            this.layer = layer;
        }

        public override readonly int GetHashCode() => HashCode.Combine(x, y);
        public override readonly bool Equals(object obj) => obj is TileInfo other && other.GetHashCode() == GetHashCode();
    }


    public enum TileLayer : byte {
        None,
        Floor,
        Wall
    }
}