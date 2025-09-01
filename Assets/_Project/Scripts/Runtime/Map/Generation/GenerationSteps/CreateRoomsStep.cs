using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation.GenerationSteps {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Create Rooms Step", order = 1)]
    internal class CreateRoomsStep : MapGenerationStep {
        [Header("Settings")]
        [SerializeField] int _numberOfRooms;
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
            for(int i = 0; i < _numberOfRooms; i++) {
                rooms.Add(GenerateRoom(0, random));
            }

            for (int i = 0; i < _numberOfPaddingRooms; i++) {
                rooms.Add(GenerateRoom(-1, random));
            }
            map.rooms = rooms.ToArray();
        }

        Room GenerateRoom(int id, Random random) {
            float angle = (float)random.NextDouble() * 360 * Mathf.Deg2Rad;
            float distance = (float)random.NextDouble() * _maxDistanceFromOrigin;
            int width = random.Next(_widthRange.x / 2, _widthRange.y / 2) * 2 + 1;
            int height = random.Next(_heightRange.x / 2, _heightRange.y / 2) * 2 + 1;

            Vector2Int size = new Vector2Int(width, height);
            Vector2 position = new(distance * Mathf.Cos(angle) - size.x / 2, distance * Mathf.Sin(angle) - size.y / 2);


            return new() {
                roomId = id,
                position = Vector2Int.RoundToInt(position),
                size = size,
                border = _border,
                margin = _margin
            };
        }
    }
}