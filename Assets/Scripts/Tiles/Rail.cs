using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Rail : TileObject
{
    public Rail(Tilemap tilemap, Vector2Int position) : base(tilemap, position) { }

    public override void OnGenerate() 
    {
        var tile = Resources.Load<Tile>("Sprites/Tiles/rail_big");
        var pos = new Vector3Int(Position.x, Position.y, 0);
        Tilemap.SetTile(pos, tile);
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
