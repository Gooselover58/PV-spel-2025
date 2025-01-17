using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        //Generate();
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
        int length = 600;
        for (int i = 0; i < length; i++)
        {
            var pos = new Vector2Int(startPos.x, startPos.y + i);
            ReplaceTile(new Rail(pos, this), GroundmapData);
        }

        for (int i = 0; i < 50; i++)
        {
            Step();
        }

        ReplaceTile(new EndTile(new Vector2Int(startPos.x, startPos.y + length), this), GroundmapData);
    }

    public void ReplaceTile(AbstractTile tile, Dictionary<Vector2Int, AbstractTile> dictionary)
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

        List<AbstractTile> values = new List<AbstractTile>();
        values.AddRange(WalkermapData.Values);

        // Makes any tiles with step method take a step
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
