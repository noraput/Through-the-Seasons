using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ThroughTheSeasons
{
    public class TileManager : MonoBehaviour
    {
        Tilemap tilemap;

        private int loopTileCount = 3;
        private int start;
        private int end;

        private bool isFirstTile;
        private int rowStartingOrder;

        private int order;
        private List<int> heads;
 
        private void Start() {
            tilemap = GetComponent<Tilemap>();
            heads = new List<int>();
            
            isFirstTile = true;

            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {   
                bool isHead = false;

                start = pos.x < start ? pos.x : start;
                end = pos.x > end ? pos.x : end;

                if (GameManager.instance.TileState == TileCreateState.Head || heads.Contains(pos.x)) {
                    if (!heads.Contains(pos.x)) {
                        heads.Add(pos.x);
                    }

                    tilemap.SetTile(pos, TileAssets.instance.headTile);
                    GameManager.instance.TileState = TileCreateState.Normal;

                    isHead = true;
                    // Debug.Log(pos + " is Head");
                }

                else if (!IsEndOfChunk(pos) && !tilemap.HasTile(pos + Vector3Int.right) && GameManager.instance.TileState != TileCreateState.Tail) {
                    tilemap.SetTile(pos, TileAssets.instance.GetTailTileAt(order));
                    GameManager.instance.TileState = TileCreateState.Tail;

                    // Debug.Log(pos + " is Tail");
                }

                else if (tilemap.HasTile(pos + Vector3Int.right) && GameManager.instance.TileState == TileCreateState.Tail) {
                    GameManager.instance.TileState = TileCreateState.Head;
                    // Debug.Log(pos + " will have " + (pos + Vector3Int.right) + " as the next head");
                }

                else if (!tilemap.HasTile(pos) && GameManager.instance.TileState == TileCreateState.Tail) {
                    // Debug.Log(pos + " does not have a tile, going next tile");
                }

                else {
                    if (pos.x == start) {
                        GameManager.instance.CurrentTileOrder = rowStartingOrder;
                        order = GameManager.instance.CurrentTileOrder % loopTileCount;

                        Debug.Log("Creating new row, Order: " + order);
                    }

                    Tile tile = TileAssets.instance.GetTileAt(order);
                    tilemap.SetTile(pos, tile);
                    Debug.Log("Pos: " + pos + " | Order: " + order + " | Status: " + GameManager.instance.TileState + " | Tile: " +  tile.name);
                }

                // Debug.Log("Pos: " + pos + " | Order: " + order + " | Status: " + GameManager.instance.TileState);

                if (isFirstTile) {
                    Debug.Log("Recorded first column");

                    rowStartingOrder = GameManager.instance.CurrentTileOrder;
                    isFirstTile = false;
                }
                
                GameManager.instance.CurrentTileOrder = isHead ? 0 : GameManager.instance.CurrentTileOrder + 1;
                order = GameManager.instance.CurrentTileOrder % loopTileCount;
            }

            // Debug.Log("Start: " + start + " | End: " + end);
        }

        private bool IsEndOfChunk(Vector3Int pos) {
            return pos.x + 1 > end;
        }

        private bool IsHeadTile(Vector3Int pos) {
            return heads.Contains(pos.x);
        }
    }
}