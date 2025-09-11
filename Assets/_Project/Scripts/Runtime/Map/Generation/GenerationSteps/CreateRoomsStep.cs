using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation.GenerationSteps {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Create Rooms Step", order = 1)]
    internal class CreateRoomsStep : MapGenerationStep {
        [Header("Settings")]
        [SerializeField] RoomClass[] _roomClasses;

        [SerializeField] int _numberOfPaddingRooms;
        [Space(10)]
        [SerializeField] Vector2Int _widthRange;
        [SerializeField] Vector2Int _heightRange;
        [SerializeField] int _border;
        [SerializeField] int _margin;
        [Space(10)]
        [SerializeField] int _maxDistanceFromOrigin;

        public override void ApplyStep(Map map, Random random) {
            List<Room> rooms = new();
            // Generate Legitimate Rooms
            /*for(int i = 0; i < _numberOfRooms; i++) {
                rooms.Add(GenerateFillerRoom(random));
            }*/

            // Generate Filler Rooms to pad the others
            for(int i = 0; i < _numberOfPaddingRooms; i++) {
                rooms.Add(GenerateFillerRoom(random));
            }
            map.rooms = rooms.ToArray();
        }

        Room[] GenerateRoom(RoomClass roomClass, Random random) {
            int numberOfRooms = random.Next(roomClass.instancesRange.x, roomClass.instancesRange.y + 1);
            Room[] rooms = new Room[numberOfRooms];
            for(int i = 0; i < numberOfRooms; i++) {
                // Get all the rooms which haven't been generated already
                RoomConfig[] options;
                if(roomClass.allowDuplicates) options = roomClass.rooms;
                else options = roomClass.rooms
                    .Where(roomConfig => !Array.Exists(rooms, r => r.roomId != roomConfig.GetInstanceID()))
                    .ToArray();


            }
            return rooms;
        }

        Room GenerateFillerRoom(Random random) {
            float angle = (float)random.NextDouble() * 360 * Mathf.Deg2Rad;
            float distance = (float)random.NextDouble() * _maxDistanceFromOrigin;
            int width = random.Next(_widthRange.x / 2, _widthRange.y / 2) * 2 + 1;
            int height = random.Next(_heightRange.x / 2, _heightRange.y / 2) * 2 + 1;

            Vector2Int size = new Vector2Int(width, height);
            Vector2 position = new(distance * Mathf.Cos(angle) - size.x / 2, distance * Mathf.Sin(angle) - size.y / 2);


            return new() {
                roomId = -1,
                tag = RoomType.Filler,
                position = Vector2Int.RoundToInt(position),
                size = size,
                border = _border,
                margin = _margin
            };
        }

        private void OnValidate() {
            if(_roomClasses == null) return;
            foreach(var roomClass in _roomClasses) {
                roomClass?.Validate();
            }
        }
    }


    [Serializable]
    public class RoomClass {
        [HideInInspector] public string name;
        public RoomType tag;
        public bool allowDuplicates;
        public Vector2Int instancesRange;
        public RoomConfig[] rooms;

        public void Validate() {
            if(!allowDuplicates) {
                instancesRange.y = Mathf.Clamp(instancesRange.y, 0, rooms.Length);
            }
            instancesRange.x = Mathf.Clamp(instancesRange.x, 0, instancesRange.y);

            name = tag.ToString();
            for(int i = 0; i < rooms.Length; i++) {
                if(rooms[i] == null) continue;
                if(rooms[i].tag != tag) rooms[i] = null;
            }
        }
    }
}