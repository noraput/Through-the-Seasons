using UnityEngine;
using UnityEngine.Tilemaps;

namespace ThroughTheSeasons
{
    public class TileAssets : PersistentObject<TileAssets>
    {
        public Tile headTile;
        public TileInfo[] tiles;
        public TileInfo[] tailTiles;
        public DoubleTiles[] grassTiles;

        public Tile GetTileAt(int order) {
            return tiles.GetTileAt(order);
        }

        public Tile GetTailTileAt(int order) {
            return tailTiles.GetTileAt(order);
        }

        public DoubleTiles GetGrassTileAt(int order) {
            return grassTiles[order];
        }
    }
}
