using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkerTile : AbstractTile
{
    private bool IsDebug;
    public WalkerTile(Vector2Int position, TilePlacer tilePlacer) : base(position, tilePlacer) { }
    public override void OnGenerate()
    {
        TilePlacer.Debugmap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/debug_walker"));

    }

    public void Step() 
    {
        
    }
}
