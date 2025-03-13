using Managers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DungeonGeneration {
    public class TilemapVisualiser : MonoBehaviour {
        [Header("References")]
        [SerializeField] Tilemap _holeTilemap;
        [SerializeField] Tilemap _floorTilemap;
        [SerializeField] Tilemap _wallTilemap;

        [Header("Tiles")]
        [SerializeField] TileBase _floorTile;
        [SerializeField] TileBase _wallTile;
        [SerializeField] TileBase _holeTile;


        public void Clear() {
            _holeTilemap.ClearAllTiles();
            _floorTilemap.ClearAllTiles();
            _wallTilemap.ClearAllTiles();
        }

        public void Fill(TileLayer layer, Vector3Int start, Vector3Int end) {
            (Tilemap tilemap, TileBase tile) = layer switch {
                TileLayer.None => (_holeTilemap, _holeTile),
                TileLayer.Floor => (_floorTilemap, _floorTile),
                TileLayer.Wall => (_wallTilemap, _wallTile),
                _ => (_holeTilemap, _holeTile)
            };

            //Determine directions on X and Y axis
            var xDir = start.x < end.x ? 1 : -1;
            var yDir = start.y < end.y ? 1 : -1;
            //How many tiles on each axis?
            int xCols = 1 + Mathf.Abs(start.x - end.x);
            int yCols = 1 + Mathf.Abs(start.y - end.y);
            //Start painting
            for (var x = 0; x < xCols; x++) {
                for (var y = 0; y < yCols; y++) {
                    var tilePos = start + new Vector3Int(x * xDir, y * yDir);
                    tilemap.SetTile(tilePos, tile);
                }
            }
        }

        public void Fill(TileLayer layer, Vector3Int position, int startX, int startY, int endX, int endY) {
            (Tilemap tilemap, TileBase tile) = layer switch {
                TileLayer.None => (_holeTilemap, _holeTile),
                TileLayer.Floor => (_floorTilemap, _floorTile),
                TileLayer.Wall => (_wallTilemap, _wallTile),
                _ => (_holeTilemap, _holeTile)
            };
            tilemap.BoxFill(position, tile, startX, startY, endX, endY);
            Logging.Log(this, $"Filled in floor from {startX}, {startY} to {endX} {endY} on the {layer} layer.");
        }

        public void PaintSingleTile(Vector2Int position, TileLayer layer) {
            (Tilemap tilemap, TileBase tile) = layer switch {
                TileLayer.None => (_holeTilemap, _holeTile),
                TileLayer.Floor => (_floorTilemap, _floorTile),
                TileLayer.Wall => (_wallTilemap, _wallTile),
                _ => (_holeTilemap, _holeTile)
            };
            if (layer == TileLayer.Floor) return;
            var tilePos = tilemap.WorldToCell((Vector3Int)position);
            tilemap.SetTile(tilePos, tile);
        }
    }
}