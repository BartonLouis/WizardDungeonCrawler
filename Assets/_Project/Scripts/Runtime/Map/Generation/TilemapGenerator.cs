using Louis.CustomPackages.Logging;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map.Generation {
    internal class TilemapGenerator : MonoBehaviour {

        [Header("References")]
        [SerializeField] TileBase _tile;
        [SerializeField] Tilemap _tilemap;

        // We cache the previous form of the map so that updating the map after each iteration is less performance impactful
        TileType[,] _previousMap;

        public void Clear() {
            _tilemap.ClearAllTiles();
        }

        public void GenerateTilemap(Map map) {
            if(!map.changedFlag) return;
            int width = map.Size.x;
            int height = map.Size.y;
            if(_previousMap == null || _previousMap.GetLength(0) != map.Size.x || _previousMap.GetLength(1) != map.Size.y) { 
                _previousMap = new TileType[width, height]; 
            }
            int halfWidth = width / 2;
            int halfHeight = height / 2;
            
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if(map[x, y] != _previousMap[x, y]) {
                        _previousMap[x, y] = map[x, y];
                        if(map[x, y] == TileType.Wall) {
                            _tilemap.SetTile(new Vector3Int(x - halfWidth, halfHeight - y, 0), _tile);
                        } else {
                            _tilemap.SetTile(new Vector3Int(x - halfWidth, halfHeight - y, 0), null);
                        }
                    }
                }
            }
            map.changedFlag = false;
        }
    }
}