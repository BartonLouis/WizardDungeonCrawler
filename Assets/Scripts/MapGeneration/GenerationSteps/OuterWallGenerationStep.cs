using UnityEngine;


namespace DungeonGeneration {
    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Outer Wall Generation Step")]
    public class OuterWallGenerationStep : AbstractGenerationStep {
        public override void Generate(DungeonInfo dungeon) {
            if (dungeon.Border <= 0) return;
            for (int x = 0; x < dungeon.Width; x++) {
                for (int w = 0; w < dungeon.Border; w++) {
                    dungeon[x, w] = new TileInfo(dungeon[x, w], TileLayer.Wall);
                    dungeon[x, dungeon.Height - w - 1] = new TileInfo(dungeon[x, dungeon.Height - w - 1], TileLayer.Wall);
                }
            }

            for (int y = 0; y < dungeon.Height; y++) {
                for (int w = 0; w < dungeon.Border; w++) {
                    dungeon[w, y] = new TileInfo(dungeon[w, y], TileLayer.Wall);
                    dungeon[dungeon.Width - w - 1, y] = new TileInfo(dungeon[dungeon.Width - w - 1, y], TileLayer.Wall);
                }
            }
        }
    }
}