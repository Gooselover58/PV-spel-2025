using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerationManager : MonoBehaviour
{
    private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
    private Tilemap groundMap;
    private Tilemap railMap;
    private Tilemap wallMap;

    private void Awake()
    {
        LoadResources();
    }

    private void LoadResources()
    {

    }
}
