using Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Random = System.Random;

namespace DungeonGeneration {
    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Room Separation Step")]
    public class RoomSeparationStep : AbstractGenerationStep {

        [Header("Settings")]
        [SerializeField] int _maxIterations;
        [Range(1f, 10f)]
        [SerializeField] float _stepWeight;


        Dungeon _dungeon;
        Random _random;

        public override void Generate(Dungeon dungeon) {
            _dungeon = dungeon;
            _random = new Random(dungeon.Seed);
            List<RoomInfo> rooms = _dungeon.Rooms.ToList();

            // Step 2: Iteratively step rooms apart from each other until none are overlapping
            Vector2[] overlapVectors = new Vector2[rooms.Count];
            int iterations = 0;
            while (rooms.AnyIntersect() && iterations < _maxIterations) {
                for (int i = 0; i < overlapVectors.Length; i++) {
                    overlapVectors[i] = new();
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
                            overlapVectors[j] += -moveVector;
                        }
                    }
                }

                for (int i = 0; i < overlapVectors.Length; i++) {
                    if (overlapVectors[i].magnitude == 0) continue;
                    Vector2 normalised = _stepWeight * overlapVectors[i].normalized;
                    RoomInfo oldRoom = rooms[i];
                    RoomInfo newRoom = new RoomInfo(oldRoom, oldRoom.bounds.position + new Vector3Int(
                        Mathf.CeilToInt(normalised.x),
                        Mathf.CeilToInt(normalised.y)
                        ));

                    rooms[i] = newRoom;
                }
                iterations++;
            }

            // Step 3: Move all rooms by a small amount to center them on the map
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

            // Step 4: Set Dungeon Rooms
            _dungeon.SetRooms(rooms.ToArray());

        }
    }
}