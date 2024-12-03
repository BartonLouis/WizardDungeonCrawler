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

        public void PaintSingleTile(Vector2Int position, TileLayer layer) {
            (Tilemap tilemap, TileBase tile) = layer switch {
                TileLayer.None => (_holeTilemap, _holeTile),
                TileLayer.Floor => (_floorTilemap, _floorTile),
                TileLayer.Wall => (_wallTilemap, _wallTile),
                _ => (_holeTilemap, _holeTile)
            };
            var tilePos = tilemap.WorldToCell((Vector3Int)position);
            
            tilemap.SetTile(tilePos, tile);
        }
    }
}