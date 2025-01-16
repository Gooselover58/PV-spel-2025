using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bridge : AbstractTile
{
    private Vector2Int direction;

    public Bridge(Vector2Int position, TilePlacer tilePlacer, Vector2Int direction) : base(position, tilePlacer)
    {
        this.direction = direction;
    }

    public override void OnGenerate()
    {
        tilePlacer.Groundmap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/bridge"));
        //tilePlacer.Overlaymap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/cave_floor_inner"));
    }
}
