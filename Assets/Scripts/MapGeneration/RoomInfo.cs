using System;
using Utils;
using UnityEngine;

namespace DungeonGeneration {
    [Serializable]
    public struct RoomInfo {
        public BoundsInt bounds;
        public int margin;
        public int border;
        public RoomType roomType;
        public DoorSide openDoors;
        public int maxOpenDoors;

        public RoomInfo(BoundsInt bounds, int margin, int border, RoomType roomType, int maxOpenDoors) {
            this.bounds = bounds;
            this.margin = margin;
            this.border = border;
            this.roomType = roomType;
            this.maxOpenDoors = maxOpenDoors;
            openDoors = 0;
        }

        public RoomInfo(RoomInfo old, Vector3Int position) {
            margin = old.margin;
            border = old.border;
            roomType = old.roomType;
            bounds = new BoundsInt(position, old.bounds.size);
            maxOpenDoors = old.maxOpenDoors;
            openDoors = old.openDoors;
        }

        public void AddDoor(DoorSide doorToAdd) {
            openDoors = openDoors | doorToAdd;
        }

        public readonly bool CanAddDoor(DoorSide doorToAdd) {
            Debug.Log($"Checking {roomType} if it can add more doors\n" +
                $"Number of open doors {openDoors.CountFlags()}\n" +
                $"Open Doors: {openDoors}\n" +
                $"Max Number of Open Doors: {maxOpenDoors}\n" +
                $"Door To Add: {doorToAdd}");
            if (openDoors.CountFlags() < maxOpenDoors) {
                Debug.Log($"{roomType} Can Add Door as there are fewer doors open than the maximum");
                return true;
            } else if (openDoors.CountFlags() == maxOpenDoors && openDoors.HasFlag(doorToAdd)) {
                Debug.Log($"{roomType} Can Add Door as there are an equal number of doors as the max but this door is already open");
                return true;
            }
            Debug.Log($"{roomType} Cannot Add Door {doorToAdd} as Max Open Doors is {maxOpenDoors} and Currently open doors are {openDoors}\n" +
                $"Number of open Doors = {openDoors.CountFlags()}");
            return false;
            return ((openDoors.CountFlags() < maxOpenDoors) 
                || (openDoors.CountFlags() == maxOpenDoors && openDoors.HasFlag(doorToAdd)));
        }

        public override readonly int GetHashCode() => HashCode.Combine(margin, border, bounds);
        public override readonly bool Equals(object obj) => obj is RoomInfo other && other.GetHashCode() == GetHashCode();
        public static bool operator ==(RoomInfo lhs, RoomInfo rhs) { return lhs.Equals(rhs); }
        public static bool operator !=(RoomInfo lhs, RoomInfo rhs) { return !lhs.Equals(rhs); }
    }

    [Flags]
    public enum DoorSide {
        Top     = 1,
        Bottom  = 2,
        Left    = 4,
        Right   = 8
    }
}