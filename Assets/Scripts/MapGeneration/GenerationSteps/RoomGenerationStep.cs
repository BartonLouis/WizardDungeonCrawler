using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace DungeonGeneration {
    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Tiny Keep Room Generation Step")]
    public class RoomGenerationStep : AbstractGenerationStep {

        [Header("Settings")]
        [SerializeField] float _placementRadius;
        [Space(5)]
        [Tooltip("The Border is the width of the walls around the room")]
        [SerializeField] int _roomBorder;
        [Tooltip("The Margin is the space around the room where no other rooms can overlap (Safe-Zone)")]
        [SerializeField] int _roomMargin;

        [Header("Rooms To Spawn")]
        [SerializeField] List<RoomGroup> _roomGroups;

        List<RoomInfo> _rooms;
        Random _random;
        Dungeon _dungeon;

        public override void Generate(Dungeon dungeon) {
            _dungeon = dungeon;
            _random = new Random(dungeon.Seed);

            _rooms = new();
            foreach (var roomGroup in _roomGroups) {
                int numberOfRooms = _random.Next(roomGroup.minNumberOfRoomsFromGroup, roomGroup.maxNumberOfRoomsFromGroup + 1);
                for (int i = 0; i < numberOfRooms; i++) {
                    GenerateRoom(roomGroup);
                }
            }

            _dungeon.SetRooms(_rooms.ToArray());
        }


        public void GenerateRoom(RoomGroup group) {
            RoomSO roomChoice = group.rooms[_random.Next(group.rooms.Length)];

            float angle;
            if (group.separationConfigs.Count == 0) {
                angle = (float)_random.NextDouble() * 360f;
            } else {
                RoomSeparationConfig config = group.separationConfigs[0];
                RoomInfo otherRoom = _rooms.FirstOrDefault(r => r.roomType == config.otherRoomType);
                if (otherRoom == default) {
                    angle = (float)_random.NextDouble() * 360f;
                } else {
                    float otherRoomAngle = Mathf.Atan2(otherRoom.bounds.center.y, otherRoom.bounds.center.x) * Mathf.Rad2Deg;
                    float minAngle = otherRoomAngle + config.minDegreeOffset;
                    float maxAngle = otherRoomAngle + 360 - config.minDegreeOffset;
                    angle = minAngle + (maxAngle - minAngle) * (float)_random.NextDouble();
                }
            }
            float r = (float)_random.NextDouble() * _placementRadius - _placementRadius / 2;
            float weight = 1 - Mathf.Abs(2 * group.weightToEdgeOfMap - 1);
            float distance = group.weightToEdgeOfMap * _placementRadius + weight * r;

            Vector3Int roomCenter = new Vector3Int(
                (int)(distance * Mathf.Cos(angle * Mathf.Deg2Rad)),
                (int)(distance * Mathf.Sin(angle * Mathf.Deg2Rad)));
            int width = roomChoice.width + 2 * _roomBorder + 2 * _roomMargin;
            int height = roomChoice.height + 2 * _roomBorder + 2 * _roomMargin;

            BoundsInt bounds = new(roomCenter - new Vector3Int(width / 2, height / 2), new Vector3Int(width, height));
            RoomInfo room = new(bounds, _roomMargin, _roomBorder, roomChoice.roomType, roomChoice.maxOpenDoors);
            _rooms.Add(room);

        }

        private void OnValidate() {
            foreach (var roomGroup in _roomGroups) {
                roomGroup.Validate();
            }
        }
    }


    #region Wrapper Classes

    [Serializable]
    public class RoomGroup {
        [HideInInspector] public string name;
        public RoomType roomType = RoomType.Normal;
        [Tooltip("A weight of 0 means the room will always be as close to the center of the map as possible\n" +
            "A weight of 1 means the map will always be as far from the center as possible\n" +
            "A weight of 0.5 means the room is placed a random distance from the center and the outer radius")]
        [Range(0.1f, 1f)]
        public float weightToEdgeOfMap;
        public int minNumberOfRoomsFromGroup;
        public int maxNumberOfRoomsFromGroup;
        public RoomSO[] rooms;
        public List<RoomSeparationConfig> separationConfigs;

        public void Validate() {
            name = $"{roomType} ({rooms.Length})";
            minNumberOfRoomsFromGroup = Mathf.Max(1, minNumberOfRoomsFromGroup);
            maxNumberOfRoomsFromGroup = Mathf.Max(minNumberOfRoomsFromGroup, maxNumberOfRoomsFromGroup);
            while (separationConfigs.Count > 1) {
                separationConfigs.RemoveAt(separationConfigs.Count - 1);
            }
            foreach (var config in separationConfigs) {
                config.Validate();
            }
        }
    }

    [Serializable]
    public class RoomSeparationConfig {
        [HideInInspector] public string name;
        public RoomType otherRoomType = RoomType.Spawn;
        [Range(0f, 180f)]
        public float minDegreeOffset;

        public RoomSeparationConfig() {
            otherRoomType = RoomType.Normal;
        }

        public void Validate() {
            name = $"{otherRoomType}: {Mathf.FloorToInt(minDegreeOffset)}";
        }
    }

    #endregion
}