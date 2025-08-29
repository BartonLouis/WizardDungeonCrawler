using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Create Rooms Step", order = 1)]
    public class CreateRoomsStep : MapGenerationStep {
        [Header("Settings")]
        [SerializeField] int _numberOfRooms;
        [Space(10)]
        [SerializeField] Vector2Int _widthRange;
        [SerializeField] Vector2Int _heightRange;
        [SerializeField] int _border;
        [SerializeField] int _margin;
        [Space(10)]
        [SerializeField] int _maxDistanceFromOrigin;

        public override bool ApplyStep(Map map, Random random) {
            List<Room> rooms = new();
            for(int i = 0; i < _numberOfRooms; i++) {
                
                float angle = (float)random.NextDouble() * 360 * Mathf.Deg2Rad;
                float distance = (float)random.NextDouble() * _maxDistanceFromOrigin;
                Vector2Int size = new Vector2Int(
                        random.Next(_widthRange.x, _widthRange.y + 1),
                        random.Next(_heightRange.x, _heightRange.y + 1)
                        );
                Vector2 position = new(distance * Mathf.Cos(angle) - size.x / 2, distance * Mathf.Sin(angle) - size.y / 2);


                rooms.Add(new() {
                    position = Vector2Int.RoundToInt(position),
                    size = new Vector2Int(
                        random.Next(_widthRange.x, _widthRange.y + 1),
                        random.Next(_heightRange.x, _heightRange.y + 1)
                        ),
                    border = _border,
                    margin = _margin
                });
            }
            map.rooms = rooms.ToArray();
            return true;
        }
    }
}