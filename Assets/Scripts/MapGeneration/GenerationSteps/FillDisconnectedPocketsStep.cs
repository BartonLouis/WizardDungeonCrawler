using Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DungeonGeneration {

    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Fill Disconnected Caves Step")]
    public class FillDisconnectedPocketsStep : AbstractGenerationStep {

        [Header("Settings")]
        [SerializeField] int maxIterations;

        public override void Generate(DungeonInfo dungeon) {
            HashSet<TileInfo> connectedTiles = new HashSet<TileInfo>();
            RoomInfo someRoom = dungeon.Rooms[0];
            TileInfo startTile = dungeon[(int)someRoom.bounds.center.x, (int)someRoom.bounds.center.y];
            HashSet<TileInfo> open = new();
            open.Add(startTile);
            TileInfo[] neighbours = new TileInfo[4];
            int iterations = 0;
            while (open.Count > 0 && iterations < maxIterations) {
                TileInfo tile = open.First();
                open.Remove(tile);
                connectedTiles.Add(tile);
                neighbours[0] = dungeon[tile.x - 1, tile.y];
                neighbours[1] = dungeon[tile.x + 1, tile.y];
                neighbours[2] = dungeon[tile.x, tile.y - 1];
                neighbours[3] = dungeon[tile.x, tile.y + 1];
                foreach(TileInfo n in neighbours) {
                    if (connectedTiles.Contains(n) || dungeon[n.x, n.y].layer == TileLayer.Wall) continue;
                    open.Add(n);
                }
                iterations++;
            }

            foreach(var tile in dungeon) { 
                if (!connectedTiles.Contains(tile)) {
                    dungeon[tile.x, tile.y] = new TileInfo(tile, TileLayer.Wall);
                }
            }
        }
    }
}