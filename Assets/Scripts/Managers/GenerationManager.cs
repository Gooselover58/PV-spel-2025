using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerationManager : MonoBehaviour
{
    private static GenerationManager instance;
    private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
    [SerializeField] Tilemap testTilemap;
    private GameObject worldTilemap;
    private Tilemap groundMap;
    private Tilemap railMap;
    private Tilemap wallMap;
    public Tile currentGroundTile;

    public static GenerationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GenerationManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        LoadResources();
        int rand = Random.Range(15, 30);
        LoadMap(rand);
    }

    private void LoadResources()
    {
        worldTilemap = GameObject.FindGameObjectWithTag("WorldTilemap");
        groundMap = worldTilemap.transform.GetChild(0).GetComponent<Tilemap>();
        railMap = worldTilemap.transform.GetChild(1).GetComponent<Tilemap>();
        wallMap = worldTilemap.transform.GetChild(2).GetComponent<Tilemap>();
        tiles.Add("Rail", Resources.Load<Tile>("Sprites/Tiles/TestRail"));
        tiles.Add("MountainG", Resources.Load<Tile>("Sprites/Tiles/TestGround"));
        currentGroundTile = tiles["MountainG"];
    }

    private void LoadMap(int length)
    {
        Vector3Int railPos = Vector3Int.zero; 
        for (int i = 0; i < length; i++)
        {
            Vector3Int walkerDir = (Random.Range(0, 2) == 0) ? Vector3Int.right : Vector3Int.left;
            new Walker(railPos, walkerDir, Random.Range(10, 25));
            PlaceRail(railPos);
            railPos += Vector3Int.up;
        }
    }

    private void PlaceRail(Vector3Int pos)
    {
        railMap.SetTile(pos, tiles["Rail"]);
        for (int i = -1; i < 2; i++)
        {
            Vector3Int groundPos = pos + new Vector3Int(i, 0, 0);
            PlaceTile(tiles["MountainG"], groundPos);
        }
    }

    public void PlaceTile(TileBase tileType, Vector3Int pos)
    {
        groundMap.SetTile(pos, tileType);
    }
}

public struct TileAndPos
{
    public TileBase tile;
    public Vector3Int pos;
}

public class TileConstruct
{
    List<TileAndPos> tiles;

    public TileConstruct(Tilemap tilemap)
    {
        Vector3Int boundPos = tilemap.cellBounds.position;
        Vector3Int boundSize = tilemap.cellBounds.size;
        for (int i = boundPos.x; i < boundSize.x; i++)
        {
            for (int j = boundPos.y; j < boundSize.y; j++)
            {
                Vector3Int tilePos = new Vector3Int(i, j, 0);
                TileBase tile = tilemap.GetTile(tilePos);
                if (tile != null)
                {
                    TileAndPos tileAndPos = new TileAndPos();
                    tileAndPos.tile = tile;
                    tileAndPos.pos = tilePos;
                    tiles.Add(tileAndPos);
                }
            }
        }
    }

    public void PlaceConstruct(Vector3Int pos)
    {
        foreach (TileAndPos tile in tiles)
        {
            Vector3Int tilePos = pos + tile.pos;
            GenerationManager.Instance.PlaceTile(tile.tile, tilePos);
        }
    }
}

public class Walker
{
    protected Vector3Int start;
    protected Vector3Int dir;
    protected int length;

    public Walker(Vector3Int _start, Vector3Int _dir, int _length)
    {
        start = _start;
        dir = _dir;
        length = _length;
        Walk();
    }

    protected void Walk()
    {
        Vector3Int pos = start;
        int turnChance = 7;
        for (int i = 0; i < length; i++)
        {
            pos += dir;
            PlaceGround(pos);
            int shouldTurn = Random.Range(0, turnChance);
            if (shouldTurn == 0)
            {
                turnChance = 10;
                dir = ChangeDirection();
            }
            else
            {
                turnChance--;
            }
        }
    }

    protected void PlaceGround(Vector3Int pos)
    {
        Vector3Int groundDir = TurnVector(dir);
        for (int i = -1; i < 2; i++)
        {
            Vector3Int groundPos = pos + (groundDir * i);
            GenerationManager.Instance.PlaceTile(GenerationManager.Instance.currentGroundTile, groundPos);
        }
    }

    protected Vector3Int ChangeDirection()
    {
        Vector3Int newDir = TurnVector(dir);
        int shouldReverseDir = Random.Range(0, 2);
        int x = (shouldReverseDir == 0) ? newDir.x : -newDir.x;
        int y = (shouldReverseDir == 0) ? newDir.y : -newDir.y;
        newDir = new Vector3Int(x, y, 0);
        return newDir;

    }

    protected Vector3Int TurnVector(Vector3Int vec)
    {
        Vector3Int turnedVec = new Vector3Int(vec.y, vec.x, 0);
        return turnedVec;
    }
}

public class RailPartWalker : Walker
{
    public RailPartWalker(Vector3Int _start, Vector3Int _dir, int _length) : base(_start, _dir, _length)
    {

    }
}