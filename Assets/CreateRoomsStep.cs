using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration {
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

        public override Map ApplyStep(Map map) {
            List<Room> rooms = new();
            for(int i = 0; i < _numberOfRooms; i++) {
                float angle = Random.Range(0, 360f) * Mathf.Deg2Rad;
                float distance = Random.Range(0, _maxDistanceFromOrigin);
                Vector2 position = new(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle));


                rooms.Add(new() {
                    position = Vector2Int.RoundToInt(position),
                    size = new Vector2Int(
                        Random.Range(_widthRange.x, _widthRange.y),
                        Random.Range(_heightRange.x, _heightRange.y)
                        ),
                    border = _border,
                    margin = _margin
                });
            }
            map.rooms = rooms.ToArray();
            return map;
        }
    }
}