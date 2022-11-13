using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ThroughTheSeasons
{
    public class TileAssets : PersistentObject<TileAssets>
    {
        public Tile headTile;
        public TileInfo[] tiles;
        public TileInfo[] tailTiles;

        public Tile GetTileAt(int order) {
            return tiles.GetTileAt(order);
        }

        public Tile GetTailTileAt(int order) {
            return tailTiles.GetTileAt(order);
        }
    }
}
