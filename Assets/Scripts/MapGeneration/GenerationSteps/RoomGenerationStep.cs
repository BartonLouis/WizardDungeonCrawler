using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Random = System.Random;

namespace DungeonGeneration {
    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Tiny Keep Room Generation Step")]
    public class RoomGenerationStep : AbstractGenerationStep {

        [Header("Settings")]
        [SerializeField] int _numRoomsPlaced;
        [SerializeField] int _maxRooms;
        [SerializeField] float _placementRadius;
        [SerializeField] int _maxIterations;
        [Range(0.05f, 1f)]
        [SerializeField] float _stepWeight;
        [Space(5)]
        [SerializeField] int _roomBorder;
        [SerializeField] int _roomMargin;
        [SerializeField] int _minWidth;
        [SerializeField] int _maxWidth;
        [SerializeField] int _minHeight;
        [SerializeField] int _maxHeight;


        Random _random;
        DungeonInfo _dungeon;

        public override void Generate(DungeonInfo dungeon) {
            _dungeon = dungeon;
            _random = new Random(dungeon.Seed);

            // Step 1: Generate _numRooms rooms within small radius circle
            List<RoomInfo> rooms = new();

            for (int i = 0; i < _numRoomsPlaced; i++) {
                float distance = (float)_random.NextDouble() * _placementRadius;
                float angle = (float)_random.NextDouble() * 2 * Mathf.PI;
                Vector3Int roomCenter = new Vector3Int(
                    (int)(distance * Mathf.Cos(angle)),
                    (int)(distance * Mathf.Sin(angle)));
                int width = _random.Next(_minWidth, _maxWidth + 1);
                int height = _random.Next(_minHeight, _maxHeight + 1);

                BoundsInt bounds = new BoundsInt(
                    roomCenter - new Vector3Int(width / 2, height / 2),
                    new Vector3Int(width, height)
                    );

                RoomInfo room = new RoomInfo() {
                    bounds = bounds,
                    border = _roomBorder,
                    margin = _roomMargin,
                };
                rooms.Add(room);
            }

            // Step 2: Iteratively step rooms apart from each other until none are overlapping
            Vector2[] overlapVectors = new Vector2[rooms.Count];
            int iterations = 0;
            while (rooms.AnyIntersect() && iterations < _maxIterations) {
                for (int i = 0; i < overlapVectors.Length; i++) {
                    overlapVectors[i] = Vector2.zero;
                }

                for (int i = 0; i < rooms.Count - 1; i++) {
                    for (int j = i + 1; j < rooms.Count; j++) {
                        if (rooms[i].bounds.Intersects(rooms[j].bounds)) {
                            Vector2 vec = (rooms[j].bounds.center - rooms[i].bounds.center);
                            float xOverlap = Mathf.Max((rooms[j].bounds.size.x / 2) + (rooms[i].bounds.size.x / 2) - vec.x, 1);
                            float yOverlap = Mathf.Max((rooms[j].bounds.size.y / 2) + (rooms[i].bounds.size.y / 2) - vec.y, 1);
                            Vector2 moveVector = new Vector2(
                                rooms[i].bounds.position.x > rooms[j].bounds.position.x ? xOverlap / 2 : -xOverlap / 2, 
                                rooms[i].bounds.position.y > rooms[j].bounds.position.y ? yOverlap / 2 : -xOverlap / 2);
                            overlapVectors[i] += moveVector;
                            overlapVectors[j] -= moveVector;
                        }
                    }
                }

                for (int i = 0; i < overlapVectors.Length; i++) {
                    rooms[i] = new RoomInfo(
                        rooms[i],
                        rooms[i].bounds.position + new Vector3Int(
                            Mathf.CeilToInt(_stepWeight * overlapVectors[i].x),
                            Mathf.CeilToInt(_stepWeight * overlapVectors[i].y))
                        );
                }
                iterations++;
            }

            // Step 3: Take a subset of the rooms to make map sparser
            rooms = rooms.Take(Mathf.Min(rooms.Count, _maxRooms)).ToList();


            // Step 4: Move all rooms by a small amount to center them on the map
            int averageX = 0;
            int averageY = 0;
            foreach (RoomInfo roomInfo in rooms) {
                averageX += (int)roomInfo.bounds.center.x;
                averageY += (int)roomInfo.bounds.center.y;
            }
            averageX /= rooms.Count;
            averageY /= rooms.Count;
            Vector3Int offset = new Vector3Int(averageX - _dungeon.Width / 2, averageY - _dungeon.Height / 2);
            for (int i = 0; i < rooms.Count; i++) {
                rooms[i] = new RoomInfo(rooms[i], rooms[i].bounds.position - offset);
            }

            //Step 5: Draw Rooms on the map
            _dungeon.SetRooms(rooms.ToArray());
        }
    }
}