using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePlacer : MonoBehaviour
{
    public Dictionary<Vector2Int, AbstractTile> GroundmapData = new Dictionary<Vector2Int, AbstractTile>();

    public Dictionary<Vector2Int, AbstractTile> WalkermapData = new Dictionary<Vector2Int, AbstractTile>();

    public Tilemap Groundmap;
    public Tilemap Overlaymap;

    public Tilemap Debugmap;

    private void Awake()
    {
        //LoadResources();
    }

    public void LoadResources()
    {
        
    }

    private void Start()
    {
        Generate();
    }

    public void Clear()
    {
        Groundmap.ClearAllTiles();
        GroundmapData.Clear();
        Overlaymap.ClearAllTiles();
        
        Debugmap.ClearAllTiles();
        WalkermapData.Clear();
    }

    public void Generate()
    {
        Clear();
        var startPos = new Vector2Int(0, 0);

        // Generate rail
        for (int i = 0; i < 40; i++)
        {
            var pos = new Vector2Int(startPos.x, startPos.y + i);
            ReplaceTile(new Rail(pos, this), GroundmapData);
        }
    }

    private void ReplaceTile(AbstractTile tile, Dictionary<Vector2Int, AbstractTile> dictionary)
    {
        dictionary.Remove(tile.Position);
        dictionary.Add(tile.Position, tile);
        tile.OnGenerate();
    }

    public void PlaceTile(AbstractTile tile, Dictionary<Vector2Int, AbstractTile> dictionary) 
    {
        if (dictionary.TryAdd(tile.Position, tile)) 
        {
            //dictionary.Add(tile.Position, tile);
            tile.OnGenerate();
        }
    }

    

    public void Step() 
    {
        var values = WalkermapData.Values;

        // Makes any walkers take a step
        foreach (AbstractTile tile in values)
        {
            var method = tile.GetType().GetMethod("Step");
            if (method != null)
            {
                method.Invoke(tile, new object[0]);
            }
        }
    }
}
