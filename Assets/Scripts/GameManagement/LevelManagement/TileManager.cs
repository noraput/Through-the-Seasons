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

        [SerializeField]
        private Tilemap waterTilemap;

        private int loopTileCount = 3;

        private int start;
        private int end;
        private int top;

        private bool isFirstTile;
        private int rowStartingOrder;

        private List<int> heads;
 
        private void Start() {
            tilemap = GetComponent<Tilemap>();
            heads = new List<int>();
            isFirstTile = true;

            tilemap.CompressBounds();
            grassTilemap.CompressBounds();
            waterTilemap.CompressBounds();

            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin) {   
                start = pos.x < start ? pos.x : start;
                end = pos.x > end ? pos.x : end;
                top = pos.y > top ? pos.y : top;
            }

            // Debug.Log("Start: " + start + " | End: " + end + " | Top: " + top);

            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin) {   
                CheckingForTile(pos);
            }

            foreach (Vector3Int pos in waterTilemap.cellBounds.allPositionsWithin) {
                CheckingForWaterTile(pos);
            }

            int currentGrassOrder = 0;
            for (int i = start; i <= end; i++ , currentGrassOrder++) {
                Vector3Int lowerPos = new Vector3Int(i, top - 1);

                if (!tilemap.HasTile(lowerPos))
                    continue;
                
                Vector3Int upperPos = lowerPos + Vector3Int.up;
                DoubleTiles tiles = TileAssets.instance.GetRandomGrassTiles();

                // currentGrassOrder %= TileAssets.instance.grassTiles.Length;
                // DoubleTiles tiles = TileAssets.instance.GetRandomGrassTileAt(currentGrassOrder);
                
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

                // Debug.Log("Pos: " + pos + " | Head" + " | Order: " + GameManager.instance.CurrentTileOrder);
            }

            else if (!IsEndOfChunk(pos) && !tilemap.HasTile(pos + Vector3Int.right) && GameManager.instance.TileState != TileCreateState.Tail) {
                tilemap.SetTile(pos, TileAssets.instance.GetTailTileAt(GameManager.instance.CurrentTileOrder));
                GameManager.instance.TileState = TileCreateState.Tail;

                // Debug.Log("Pos: " + pos + " | Tail" + " | Order: " + GameManager.instance.CurrentTileOrder);
            }

            else if (tilemap.HasTile(pos + Vector3Int.right) && GameManager.instance.TileState == TileCreateState.Tail) {
                GameManager.instance.TileState = TileCreateState.Head;

                // Debug.Log("Pos: " + pos + " | To Head" + " | Order: " + GameManager.instance.CurrentTileOrder);
            }

            else if (GameManager.instance.TileState == TileCreateState.Normal) {
                if (pos.x == start && !isFirstTile) {
                    GameManager.instance.CurrentTileOrder = rowStartingOrder;

                    tilemap.SetTile(pos, TileAssets.instance.GetTileAt(GameManager.instance.CurrentTileOrder));
                    // Debug.Log("Pos: " + pos + " | Back to first collumn | Order: " + GameManager.instance.CurrentTileOrder);
                }
                else {
                    Tile tile = TileAssets.instance.GetTileAt(GameManager.instance.CurrentTileOrder);
                    tilemap.SetTile(pos, tile);

                    // Debug.Log("Pos: " + pos + " | Normal Tile | Order: " + GameManager.instance.CurrentTileOrder);
                }                
            }

            // Debug.Log("Pos: " + pos + " | Order: " + order + " | Status: " + GameManager.instance.TileState + " | HasTile: " + tilemap.HasTile(pos) + " | NextTile: " + tilemap.HasTile(pos + Vector3Int.right));

            if (isFirstTile) {
                rowStartingOrder = GameManager.instance.CurrentTileOrder;
                isFirstTile = false;
            }

            GameManager.instance.CurrentTileOrder = isHead ? 0 : (GameManager.instance.CurrentTileOrder + 1) % loopTileCount;
        }

        private void CheckingForWaterTile(Vector3Int pos) {
            // Debug.Log(pos);

            Vector3 waterWorldPos = waterTilemap.CellToWorld(pos);
            Vector3Int normalTileCellPos = tilemap.WorldToCell(waterWorldPos);

            if (tilemap.HasTile(normalTileCellPos)) {
                waterTilemap.SetTile(pos, TileAssets.instance.GetMatchingTile(tilemap.GetTile<Tile>(normalTileCellPos)));
            }
            else {
                waterTilemap.SetTile(pos, TileAssets.instance.GetRandomWaterTile());
            }
        }

        public void Initialize(Season season) {
            grassTilemap.color = TileAssets.instance.GetGrassColorFromSeason(season);
        }

        private bool IsEndOfChunk(Vector3Int pos) {
            return pos.x + 1 > end;
        }

        private bool IsHeadTile(Vector3Int pos) {
            return heads.Contains(pos.x);
        }
    }
}