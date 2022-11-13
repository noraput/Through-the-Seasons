using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Extension
{
    public static Tile GetTileAt(this TileInfo[] tiles, int order)
    {
        return tiles.First(tile => tile.position == order).tile;
    }
}