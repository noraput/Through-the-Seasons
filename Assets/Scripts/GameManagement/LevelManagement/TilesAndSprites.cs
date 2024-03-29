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
    public class GrassColor
    {
        public Season season;
        public Color color;
    }

    [Serializable]
    public class ItemSpriteInfo
    {
        public ItemType itemType;
        public Sprite sprite;
        public Vector3 offset;
        public Vector3 scale;
        public Vector3 rotation;
    }
    

    [Serializable]
    public class TreePool : SeasonalPool<GameObject> {}
}
