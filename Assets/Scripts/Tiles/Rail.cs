using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Rail : AbstractTile
{
    public Rail(Vector2Int position, TilePlacer tilePlacer) : base(position, tilePlacer) { }
    public override void OnGenerate() 
    {
        TilePlacer.Groundmap.SetTile(new Vector3Int(Position.x, Position.y, 0), Resources.Load<Tile>("Tiles/rail_big"));

        var pos = new Vector2Int(Position.x + 1, Position.y);
        TilePlacer.PlaceTile(new WalkerTile(pos, TilePlacer), TilePlacer.WalkermapData);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
