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
    private bool isActive = true;

    public WalkerTile(Vector2Int position, TilePlacer tilePlacer, Vector2Int direction) : base(position, tilePlacer) 
    {
        this.direction = direction;
    }
    public override void OnGenerate()
    {
        walkerPos = Position;
        lifeTime = Random.Range(10, 20);
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
        if (isActive)
        {
            lifeTime--;

            if (!tilePlacer.GroundmapData.ContainsKey(walkerPos)) tilePlacer.PlaceTile(new FloorPlacer(walkerPos, tilePlacer, true), tilePlacer.WalkermapData);
            walkerPos += direction;

            if (Random.Range(0, turnChance) == 0)
            {
                Turn();
            }
            else
            {
                turnChance--;
            }

            if (lifeTime <= 0) isActive = false;
        }
        
    }
}
