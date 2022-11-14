using System.Linq;
using System.Collections.Generic;
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
        public TreePool[] treePools;

        public Tile GetTileAt(int order) {
            return tiles.GetTileAt(order);
        }

        public Tile GetTailTileAt(int order) {
            return tailTiles.GetTileAt(order);
        }

        public DoubleTiles GetRandomGrassTiles() {
            return grassTiles.PickRandom();
        }

        public DoubleTiles GetGrassTilesAt(int order) {
            return grassTiles[order];
        }

        public List<GameObject> GetTreePoolFromSeason(Season season) {
            Debug.Log(season);
            return treePools.First(pool => pool.season == season).pool;
        }
    }
}
