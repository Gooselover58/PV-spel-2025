using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkerTile : AbstractTile
{
    private bool IsDebug;
    private Vector2Int Direction;
    public WalkerTile(Vector2Int position, TilePlacer tilePlacer, Vector2Int direction) : base(position, tilePlacer) 
    {
        Direction = direction;
    }
    public override void OnGenerate()
    {
        RenderTile(Position);
    }

    private void RenderTile(Vector2Int position)
    {
        TilePlacer.Debugmap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/debug_walker"));
    }

    public void Step() 
    {
        Position += Direction;
        RenderTile(Position);
    }
}
