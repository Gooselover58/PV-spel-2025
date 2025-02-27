using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveFloor : AbstractTile
{
    public CaveFloor(Vector2Int position, TilePlacer tilePlacer) : base(position, tilePlacer) { }

    public override void OnGenerate() 
    {
        tilePlacer.Groundmap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/cave_floor"));
        tilePlacer.Overlaymap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/cave_floor_inner"));
    }
}
