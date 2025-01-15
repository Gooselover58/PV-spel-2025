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

    private void Turn() 
    {
        Vector2Int turnedVec = new Vector2Int(Direction.y, Direction.x);
        int shouldReverseDir = Random.Range(0, 2);
        int x = (shouldReverseDir == 0) ? turnedVec.x : -turnedVec.x;
        int y = (shouldReverseDir == 0) ? turnedVec.y : -turnedVec.y;
        Direction = new Vector2Int(x, y);
    }

    public void Step() 
    {
        int turnChance = 5;

        TilePlacer.PlaceTile(new CaveFloor(Position, TilePlacer), TilePlacer.OverlaymapData);
        Position += Direction;
        
        if (Random.Range(0, turnChance) == 0)
        {
            Turn();
        }
        else
        {
            turnChance--;
        }
    }
}
