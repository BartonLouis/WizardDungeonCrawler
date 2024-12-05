using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Random = System.Random;

namespace DungeonGeneration {


    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Room Generation Step")]
    public class BinaryPartitionRoomGenerationStep : AbstractGenerationStep {

        [Header("Settings")]
        [SerializeField] int _minRoomWidth;
        [SerializeField] int _minRoomHeight;
        [Tooltip("The minimum border between 2 rooms")]
        [SerializeField] int _margin;
        [Tooltip("The thickness of the rooms walls")]
        [SerializeField] int _border;
        [Space(5)]
        [SerializeField] int _maxRooms;


        int _doubleMinWidth;
        int _doubleMinHeight;

        Random _random;

        public override void Generate(Dungeon dungeon) {
            _doubleMinWidth = 2 * _minRoomWidth;
            _doubleMinHeight = 2 * _minRoomHeight;

            _random = new(dungeon.Seed);
            BoundsInt bounds = new BoundsInt(
                    (Vector3Int)dungeon.Center,
                    new Vector3Int(dungeon.Width, dungeon.Height)
                );

            List<RoomInfo> rooms = BinarySpacePartition(bounds);
            rooms.Shuffle(_random);
            rooms = rooms.Take(Mathf.Min(_maxRooms, rooms.Count())).ToList();
            foreach (RoomInfo room in rooms) {
                dungeon.DrawRoom(room);
            }
        }

        List<RoomInfo> BinarySpacePartition(BoundsInt space) {
            // Base condition: Room cannot divide any further
            if (space.size.x < _doubleMinWidth && space.size.y < _doubleMinHeight) {
                return new() {
                    new RoomInfo(){
                    bounds = space,
                    border = _border,
                    margin = _margin,
                    }
                };
            }

            // Choose to split vertically or horizontally
            int choice;
            if (space.size.x >= _doubleMinWidth && space.size.y < _doubleMinHeight) {
                choice = 0;
            } else if (space.size.y >= _doubleMinHeight && space.size.x < _doubleMinWidth) {
                choice = 1;
            } else {
                choice = _random.Next(1);
            }
            (BoundsInt, BoundsInt) segments = choice == 0 ? SplitHorizontal(space) : SplitVertical(space);

            // Partition Children Recursively and merge results into one list
            List<RoomInfo> result = new();
            result.AddRange(BinarySpacePartition(segments.Item1));
            result.AddRange(BinarySpacePartition(segments.Item2));
            return result;
        }

        (BoundsInt, BoundsInt) SplitHorizontal(BoundsInt space) {
            var xSplit = _random.Next(_minRoomWidth, space.size.x - _minRoomWidth);
            BoundsInt r1 = new(space.min, new Vector3Int(xSplit, space.size.y));
            BoundsInt r2 = new(new Vector3Int(space.min.x + xSplit, space.min.y),
                new Vector3Int(space.size.x - xSplit, space.size.y));
            return (r1, r2);
        }

        (BoundsInt, BoundsInt) SplitVertical(BoundsInt space) {
            var ySplit = _random.Next(_minRoomHeight, space.size.y - _minRoomHeight);
            BoundsInt r1 = new(space.min, new Vector3Int(space.size.x, ySplit));
            BoundsInt r2 = new(new Vector3Int(space.min.x, space.min.y + ySplit),
                new Vector3Int(space.size.x, space.size.y - ySplit));
            return (r1, r2);
        }
    }
}