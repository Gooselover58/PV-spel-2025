using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//using static UnityEditor.PlayerSettings;
// Temporary fix to build

public abstract class AbstractTile
{
    public Vector2Int Position;
    protected TilePlacer TilePlacer;

    public AbstractTile(Vector2Int position, TilePlacer tilePlacer)
    {
        Position = position;
        TilePlacer = tilePlacer;
    }

    //public abstract string Tile();

    public abstract void OnGenerate();
}
