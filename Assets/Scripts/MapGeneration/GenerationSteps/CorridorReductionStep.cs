using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Random = System.Random;

namespace DungeonGeneration {

    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Reduce Corridors Step")]
    public class CorridorReductionStep : AbstractGenerationStep {

        [Header("Settings")]
        [Range(0, 1)]
        [SerializeField] float _connectivity;
        RoomInfo[] _rooms;
        Dungeon _dungeon;
        Random _random;

        public override void Generate(Dungeon dungeon) {
            _dungeon = dungeon;
            _random = new Random(_dungeon.Seed);
            _rooms = _dungeon.Rooms.ToArray();
            List<CorridorInfo> finalCorridors = new();
            List<CorridorInfo> corridors = _dungeon.Corridors.ToList();

            // Step 1 : Perform Prims algorithm to create a minimum spanning tree so all rooms are reachable
            HashSet<int> connectedRooms = new() { 0 };
            List<CorridorInfo> connected = corridors
                .Where(c => IsConnectedOnOneEnd(c, connectedRooms) && CanAddCorridor(c))
                .OrderBy(c => c.Length(_dungeon))
                .ToList();

            while (connected.Count > 0) {
                connected = connected.OrderBy(c => c.Length(_dungeon)).ToList();
                CorridorInfo choice = connected.First();
                CreateDoorsOnRoom(choice);
                finalCorridors.Add(choice);
                connectedRooms.Add(choice.startRoomIndex);
                connectedRooms.Add(choice.endRoomIndex);
                connected = corridors
                    .Where(c => IsConnectedOnOneEnd(c, connectedRooms) && CanAddCorridor(c))
                    .OrderBy(c => c.Length(_dungeon))
                    .ToList();
            }

            // Step 2 : Add a random selection of other corridors to introduce loops
            int numToTake = (int)(corridors.Count * _connectivity);
            for (int i = 0; i < numToTake; i++) {
                corridors = corridors.Where(c => !finalCorridors.Contains(c) && CanAddCorridor(c)).ToList();
                corridors.Shuffle(_random);
                finalCorridors.Add(corridors[0]);
                CreateDoorsOnRoom(corridors[0]);
                corridors.RemoveAt(0);
                if (corridors.Count == 0) break;
            }



            _dungeon.SetRooms(_rooms);
            _dungeon.SetCorridors(finalCorridors.ToArray());
        }

        bool IsConnectedOnOneEnd(CorridorInfo corridor, HashSet<int> connectedRooms) {
            bool startConnected = connectedRooms.Contains(corridor.startRoomIndex);
            bool endConnected = connectedRooms.Contains(corridor.endRoomIndex);
            // XOR to ensure only one is connected
            return startConnected ^ endConnected;
        }

        bool CanAddCorridor(CorridorInfo corridor) {
            RoomInfo start = _rooms[corridor.startRoomIndex];
            RoomInfo end = _rooms[corridor.endRoomIndex];
            return start.CanAddDoor(corridor.startSide) && end.CanAddDoor(corridor.endSide);
        }

        void CreateDoorsOnRoom(CorridorInfo corridor) {
            _rooms[corridor.startRoomIndex].AddDoor(corridor.startSide);
            _rooms[corridor.endRoomIndex].AddDoor(corridor.endSide);
        }
    }
}