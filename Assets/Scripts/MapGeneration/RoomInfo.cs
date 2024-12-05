using System;
using UnityEngine;

namespace DungeonGeneration {
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

        public override readonly int GetHashCode() => HashCode.Combine(margin, border, bounds);
        public override readonly bool Equals(object obj) => obj is RoomInfo other && other.GetHashCode() == GetHashCode();
        public static bool operator ==(RoomInfo lhs, RoomInfo rhs) { return lhs.Equals(rhs); }
        public static bool operator !=(RoomInfo lhs, RoomInfo rhs) { return !lhs.Equals(rhs); }
    }
}