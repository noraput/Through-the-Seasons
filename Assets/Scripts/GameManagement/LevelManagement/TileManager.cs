using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ThroughTheSeasons
{
    public class TileManager : MonoBehaviour
    {
        private Tilemap tilemap;

        [SerializeField]
        private Tilemap grassTilemap;

        private int loopTileCount = 3;

        private int start;
        private int end;
        private int top;

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
                start = pos.x < start ? pos.x : start;
                end = pos.x > end ? pos.x : end;
                top = pos.y > top ? pos.y : top;
            }

            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {   
                CheckingForTile(pos);
            }

            int currentGrassOrder = 0;
            for (int i = start; i <= end; i++ , currentGrassOrder++) {
                Vector3Int lowerPos = new Vector3Int(i, top - 1);

                if (!tilemap.HasTile(lowerPos))
                    continue;
                
                Vector3Int upperPos = lowerPos + Vector3Int.up;

                currentGrassOrder %= TileAssets.instance.grassTiles.Length;
                DoubleTiles tiles = TileAssets.instance.GetGrassTileAt(currentGrassOrder);
                
                grassTilemap.SetTile(lowerPos, tiles.lower);
                grassTilemap.SetTile(upperPos, tiles.upper);
            }
        }

        private void CheckingForTile(Vector3Int pos) {
            bool isHead = false;

            if (GameManager.instance.TileState == TileCreateState.Head || heads.Contains(pos.x)) {
                if (!heads.Contains(pos.x)) {
                    heads.Add(pos.x);
                }

                tilemap.SetTile(pos, TileAssets.instance.headTile);
                GameManager.instance.TileState = TileCreateState.Normal;

                isHead = true;
            }

            else if (!IsEndOfChunk(pos) && !tilemap.HasTile(pos + Vector3Int.right) && GameManager.instance.TileState != TileCreateState.Tail) {
                tilemap.SetTile(pos, TileAssets.instance.GetTailTileAt(order));
                GameManager.instance.TileState = TileCreateState.Tail;
            }

            else if (tilemap.HasTile(pos + Vector3Int.right) && GameManager.instance.TileState == TileCreateState.Tail) {
                GameManager.instance.TileState = TileCreateState.Head;
            }

            else if (GameManager.instance.TileState == TileCreateState.Normal) {
                if (pos.x == start) {
                    GameManager.instance.CurrentTileOrder = rowStartingOrder;
                    order = GameManager.instance.CurrentTileOrder % loopTileCount;
                }

                Tile tile = TileAssets.instance.GetTileAt(order);
                tilemap.SetTile(pos, tile);
            }

            if (isFirstTile) {
                rowStartingOrder = GameManager.instance.CurrentTileOrder;
                isFirstTile = false;
            }

            Debug.Log("Pos: " + pos + " | Status: " + GameManager.instance.TileState + " | HasTile: " + tilemap.HasTile(pos) + " | NextTile: " + tilemap.HasTile(pos + Vector3Int.right));

            GameManager.instance.CurrentTileOrder = isHead ? 0 : GameManager.instance.CurrentTileOrder + 1;
            order = GameManager.instance.CurrentTileOrder % loopTileCount;
        }

        private bool IsEndOfChunk(Vector3Int pos) {
            return pos.x + 1 > end;
        }

        private bool IsHeadTile(Vector3Int pos) {
            return heads.Contains(pos.x);
        }
    }
}