using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorPlacer : AbstractTile
{
    private bool canDuplicate;
    public FloorPlacer(Vector2Int position, TilePlacer tilePlacer, bool canDuplicate) : base(position, tilePlacer) 
    {
        this.canDuplicate = canDuplicate;
    }

    public override void OnGenerate()
    {
        tilePlacer.Debugmap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/debug_floor"));

        GenerateFloor();
        //tilePlacer.PlaceTile(new CaveFloor(Position, tilePlacer), tilePlacer.GroundmapData);
    }

    private void GenerateFloor()
    {
        var startPos = new Vector2Int(Position.x -1, Position.y -1);

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                PlaceFloor(x, y, startPos);
            }
        }
    }

    private void PlaceFloor(int x, int y, Vector2Int startPos)
    {
        var currentPos = new Vector2Int(startPos.x + x, startPos.y + y);
        if (!tilePlacer.GroundmapData.ContainsKey(currentPos))
        {
            if (Random.Range(1, 5) == 1 && canDuplicate)
            {
                tilePlacer.PlaceTile(new FloorPlacer(new Vector2Int(Position.x - 1, Position.y), tilePlacer, false), tilePlacer.WalkermapData);
            }
            tilePlacer.PlaceTile(new CaveFloor(currentPos, tilePlacer), tilePlacer.GroundmapData);
            
        }
    }
}
