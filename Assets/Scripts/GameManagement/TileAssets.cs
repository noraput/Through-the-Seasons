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
        public Tile[] waterTiles;
        public MatchingTileInfo[] coveringWaterTiles;

        public TreePool[] treePools;
        public GrassColor[] grassColors;

        public Tile GetTileAt(int order) {
            return tiles.GetTileAt(order);
        }

        public Tile GetTailTileAt(int order) {
            return tailTiles.GetTileAt(order);
        }

        public Tile GetMatchingTile(Tile tile) {
            // Debug.Log(tile.name);
            return coveringWaterTiles.First(t => tile == t.matchingTile).tile;
        }
 
        public DoubleTiles GetRandomGrassTiles() {
            return grassTiles.PickRandom();
        }

        public Tile GetRandomWaterTile() {
            return waterTiles.PickRandom();
        }

        public DoubleTiles GetGrassTilesAt(int order) {
            return grassTiles[order];
        }

        public Color GetGrassColorFromSeason(Season season) {
            // Debug.Log(season);
            GrassColor grassColor = grassColors.FirstOrDefault(color => color.season == season);
            return grassColor != null ? grassColor.color : Color.white;
        }

        public List<GameObject> GetTreePoolFromSeason(Season season) {
            // Debug.Log(season);
            TreePool trees = treePools.FirstOrDefault(pool => pool.season == season);
            return (trees != null) ? trees.pool : null;
        }
    }
}
