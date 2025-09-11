using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation.GenerationSteps {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Remove Disconnected Sections", order = 13)]
    public class RemoveDisconnectedSectionsStep : MapGenerationStep {
        public override void ApplyStep(Map map, Random random) {
            HashSet<(int, int)> connectedTiles = new();
            var room = map.rooms[0];
            var startPos = room.position;
            (int, int) pos = (startPos.x + map.Size.x / 2, map.Size.y / 2 - startPos.y);
            HashSet<(int, int)> open = new() {pos};
            (int, int)[] neighbours = new (int, int)[4];
            while(open.Count > 0) {
                (int x, int y) tile = open.First();
                open.Remove(tile);
                connectedTiles.Add(tile);
                neighbours[0] = (tile.x - 1, tile.y);
                neighbours[1] = (tile.x + 1, tile.y);
                neighbours[2] = (tile.x, tile.y - 1);
                neighbours[3] = (tile.x, tile.y + 1);
                foreach((int x, int y) n in neighbours) {
                    if(n.x < 0 || n.x >= map.Size.x || n.y < 0 || n.y >= map.Size.y) continue;
                    if(map[n.x, n.y] == TileType.Wall) continue;
                    if(connectedTiles.Contains(n)) continue;
                    open.Add(n);
                }
            }

            for (int i = 0; i < map.tiles.Length; i++) {
                (int x, int y) = map.Get2DIndexes(i);
                if(!connectedTiles.Contains((x, y))) map[x, y] = TileType.Wall;
            }
            map.changedFlag = true;
        }
    }
}

/*
 [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Fill Disconnected Caves Step")]
    public class FillDisconnectedPocketsStep : AbstractGenerationStep {


        public override void Generate(Dungeon dungeon) {
            HashSet<TileInfo> connectedTiles = new HashSet<TileInfo>();
            RoomInfo someRoom = dungeon.Rooms[0];
            TileInfo startTile = dungeon[(int)someRoom.bounds.center.x, (int)someRoom.bounds.center.y];
            HashSet<TileInfo> open = new() {
                startTile
            };
            TileInfo[] neighbours = new TileInfo[4];
            while (open.Count > 0) {
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
            }

            foreach(var tile in dungeon) { 
                if (!connectedTiles.Contains(tile)) {
                    TileInfo t = tile;
                    t.layer = TileLayer.Wall;
                    dungeon[tile.x, tile.y] = t;
                }
            }
        }
    }*/