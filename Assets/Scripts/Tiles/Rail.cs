using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class Rail : AbstractTile
{
    public Rail(Vector2Int position, TilePlacer tilePlacer) : base(position, tilePlacer) { }
    public override void OnGenerate()
    {
        tilePlacer.Groundmap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/cave_floor"));
        tilePlacer.Overlaymap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/rail"));

        if (Random.Range(1, 5) == 1)
        {
            GenerateWalker();
        }
    }

    private void GenerateFloor() 
    {
        if (Random.Range(1, 5) != 1)
        {
            tilePlacer.PlaceTile(new CaveFloor(new Vector2Int(Position.x + 1, Position.y), tilePlacer), tilePlacer.GroundmapData);
        }
        else
        {
            tilePlacer.PlaceTile(new FloorPlacer(new Vector2Int(Position.x + 1, Position.y), tilePlacer), tilePlacer.WalkermapData);
        }

        if (Random.Range(1, 5) != 1) 
        { 
            tilePlacer.PlaceTile(new CaveFloor(new Vector2Int(Position.x - 1, Position.y), tilePlacer), tilePlacer.GroundmapData); 
        }
        else
        {
            tilePlacer.PlaceTile(new FloorPlacer(new Vector2Int(Position.x - 1, Position.y), tilePlacer), tilePlacer.WalkermapData);
        }
    }

    private void GenerateWalker()
    {
        // Randomly chooses 1 or -1
        var dir = (Random.value > 0.5) ? 1 : -1;

        var pos = new Vector2Int(Position.x + dir, Position.y);
        tilePlacer.PlaceTile(new WalkerTile(pos, tilePlacer, new Vector2Int(dir, 0)), tilePlacer.WalkermapData);
    }
}
