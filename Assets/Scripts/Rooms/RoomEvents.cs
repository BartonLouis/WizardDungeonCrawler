using DungeonGeneration;
using Louis.Patterns.EventSystem;
using UnityEngine;

namespace Events.Rooms {
    public readonly struct RoomEnteredEvent : IEvent {
        public readonly RoomInfo roomInfo;
        public readonly Bounds colliderBounds;
        public RoomEnteredEvent(RoomInfo roomInfo, Bounds colliderBounds) {
            this.roomInfo = roomInfo;
            this.colliderBounds = colliderBounds;
        }
    }

    public readonly struct RoomExitedEvent : IEvent {
        public readonly RoomInfo roomInfo;
        public readonly Bounds colliderBounds;
        public RoomExitedEvent(RoomInfo roomInfo, Bounds colliderBounds) {
            this.roomInfo = roomInfo;
            this.colliderBounds = colliderBounds;
        }
    }
}