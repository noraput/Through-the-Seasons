using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ThroughTheSeasons
{
    [Serializable]
    public class TileInfo
    {
        public int position;
        public Tile tile;
    }

    [Serializable]
    public class MatchingTileInfo
    {
        public Tile matchingTile;
        public Tile tile;
    }

    [Serializable]
    public class DoubleTiles
    {
        public Tile lower;
        public Tile upper;
    }

    [Serializable]
    public class TreePool : SeasonalPool<GameObject> {}
}
