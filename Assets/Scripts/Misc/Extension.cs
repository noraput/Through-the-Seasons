using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ThroughTheSeasons
{
    public static class Extension
    {
        public static Tile GetTileAt(this TileInfo[] tiles, int order)
        {
            return tiles.First(tile => tile.position == order).tile;
        }

        public static T PickRandom<T>(this List<T> list) {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T PickRandom<T>(this T[] array) {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static bool CompareLayer(this GameObject go, string layerName) {
            return LayerMask.LayerToName(go.layer) == layerName; 
        }
    }
}