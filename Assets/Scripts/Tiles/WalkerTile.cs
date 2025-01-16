using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkerTile : AbstractTile
{
    private bool IsDebug;
    private Vector2Int direction;
    private int lifeTime;
    private Vector2Int walkerPos;

    private int turnChance = 10;
    public WalkerTile(Vector2Int position, TilePlacer tilePlacer, Vector2Int direction) : base(position, tilePlacer) 
    {
        this.direction = direction;
    }
    public override void OnGenerate()
    {
        walkerPos = Position;

        RenderTile();
        lifeTime = Random.Range(10, 20);
    }

    private void RenderTile()
    {
        tilePlacer.Debugmap.SetTile(new Vector3Int(walkerPos.x, walkerPos.y, 0), Resources.Load<Tile>("Tiles/debug_walker"));
    }

    private void Turn() 
    {
        Vector2Int turnedVec = new Vector2Int(direction.y, direction.x);
        int shouldReverseDir = Random.Range(0, 2);
        int x = (shouldReverseDir == 0) ? turnedVec.x : -turnedVec.x;
        int y = (shouldReverseDir == 0) ? turnedVec.y : -turnedVec.y;
        direction = new Vector2Int(x, y);
        turnChance = 5;
    }

    public void Step() 
    {
        lifeTime--;

        if (!tilePlacer.GroundmapData.ContainsKey(walkerPos)) tilePlacer.PlaceTile(new CaveFloor(walkerPos, tilePlacer), tilePlacer.GroundmapData);
        walkerPos += direction;
        
        if (Random.Range(0, turnChance) == 0)
        {
            Turn();
        }
        else
        {
            turnChance--;
        }

        if (lifeTime <= 0) { tilePlacer.WalkermapData.Remove(Position); }
    }
}
