using System;
using UnityEngine;
using Utils;

namespace DungeonGeneration {
    [Serializable]
    public struct CorridorInfo {
        public int startRoomIndex;
        public int endRoomIndex;
        public Side startSide;
        public Side endSide;
        public Vector2 startDoor;
        public Vector2 endDoor;

        public CorridorInfo(int startRoomIndex, int endRoomIndex, Side startSide, Side endSide, Dungeon dungeon) {
            this.startRoomIndex = startRoomIndex;
            this.endRoomIndex = endRoomIndex;
            this.startSide = startSide;
            this.endSide = endSide;
            RoomInfo startRoom = dungeon.Rooms[startRoomIndex];
            RoomInfo endRoom = dungeon.Rooms[endRoomIndex];
            startDoor = startRoom.GenerateDoor(startSide);
            endDoor = endRoom.GenerateDoor(endSide);
        }

        public readonly override bool Equals(object obj) => obj is CorridorInfo other && other.startRoomIndex == startRoomIndex && other.endRoomIndex == endRoomIndex;
        public readonly override int GetHashCode() => HashCode.Combine(startRoomIndex, endRoomIndex, startSide, endSide);

        public static bool operator == (CorridorInfo lhs, CorridorInfo rhs) => 
            (lhs.startRoomIndex == rhs.startRoomIndex && lhs.endRoomIndex == rhs.endRoomIndex) ||
            (lhs.startRoomIndex == rhs.endRoomIndex && lhs.endRoomIndex == rhs.startRoomIndex);
        public static bool operator != (CorridorInfo lhs, CorridorInfo rhs) => !(lhs == rhs);
    }
}