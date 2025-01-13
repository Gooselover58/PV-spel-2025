using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacer : MonoBehaviour
{
    private Dictionary<Vector2Int, TileObject> TilemapData = new Dictionary<Vector2Int, TileObject>();
    private Tilemap Tilemap;

    private void Awake()
    {
        Tilemap = GetComponent<Tilemap>();
    }

    private void Start()
    {
        Generate();
    }

    public void Clear()
    {
        Tilemap.ClearAllTiles();
        TilemapData.Clear();
    }

    public void Generate()
    {
        //Tilemap = GetComponent<Tilemap>();
        Tilemap.ClearAllTiles();
        TilemapData.Clear();
        var pos = new Vector2Int(0, 0);
        var tile = new Rail(Tilemap, pos);
        TilemapData.Add(pos, tile);
        tile.OnGenerate();

    }
}
