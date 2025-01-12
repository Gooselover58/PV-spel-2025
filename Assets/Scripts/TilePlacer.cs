using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacer : MonoBehaviour
{
    private Dictionary<Vector2Int, TileObject> TilemapData = new Dictionary<Vector2Int, TileObject>();
    private Tilemap Tilemap;

    private void Start()
    {
        Tilemap = GetComponent<Tilemap>();
    }

    public void Generate()
    {
        TilemapData.Clear();
        var pos = new Vector2Int(0, 0);
        var tile = new Rail(Tilemap, pos);
        TilemapData.Add(pos, tile);
        tile.OnGenerate();

    }
}
