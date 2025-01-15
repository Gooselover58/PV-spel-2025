using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Rail : AbstractTile
{
    public Rail(Vector2Int position, TilePlacer tilePlacer) : base(position, tilePlacer) { }
    public override void OnGenerate() 
    {
        TilePlacer.Groundmap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/cave_floor"));
        TilePlacer.Overlaymap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/rail"));

        if (Random.Range(1, 5) == 1) GenerateWalker();
    }

    private void GenerateWalker()
    {
        // Randomly chooses 1 or -1
        var dir = (Random.value > 0.5) ? 1 : -1;

        var pos = new Vector2Int(Position.x + dir, Position.y);
        TilePlacer.PlaceTile(new WalkerTile(pos, TilePlacer, new Vector2Int(dir, 0)), TilePlacer.WalkermapData);
    }
}
