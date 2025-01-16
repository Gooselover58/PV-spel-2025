using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorPlacer : AbstractTile
{
    public FloorPlacer(Vector2Int position, TilePlacer tilePlacer) : base(position, tilePlacer) { }

    public override void OnGenerate()
    {
        tilePlacer.Debugmap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/debug_floor"));

        PlaceFloor();
    }

    private void PlaceFloor()
    {
        
    }
}
