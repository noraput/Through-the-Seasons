using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class TileInfo
{
    public int position;
    public Tile tile;
}

[Serializable]
public class DoubleTiles
{
    public Tile lower;
    public Tile upper;
}
