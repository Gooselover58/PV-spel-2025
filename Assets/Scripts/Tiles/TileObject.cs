using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public abstract class TileObject
{
    protected Vector2Int Position;
    protected Tilemap Tilemap;

    public TileObject(Tilemap tilemap, Vector2Int position)
    {
        Tilemap = tilemap;
        Position = position;
    }

    //public abstract string Tile();

    public abstract void OnGenerate();
}
