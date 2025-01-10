using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerationManager : MonoBehaviour
{
    private static GenerationManager instance;
    public static int maxY;
    public static int minY;
    private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
    private Dictionary<string, TileConstruct> tileConstructs = new Dictionary<string, TileConstruct>();
    private Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();
    private GameObject worldTilemap;
    private Tilemap groundMap;
    private Tilemap railMap;
    private Tilemap wallMap;
    [HideInInspector] public Tile currentGroundTile;

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
        LoadMap();
    }

    private void LoadResources()
    {
        worldTilemap = GameObject.FindGameObjectWithTag("WorldTilemap");
        groundMap = worldTilemap.transform.GetChild(0).GetComponent<Tilemap>();
        railMap = worldTilemap.transform.GetChild(1).GetComponent<Tilemap>();
        wallMap = worldTilemap.transform.GetChild(2).GetComponent<Tilemap>();
        tiles.Add("Rail", Resources.Load<Tile>("Sprites/Tiles/TestRail"));
        tiles.Add("MountainG", Resources.Load<Tile>("Sprites/Tiles/TestGround"));
        tileConstructs.Add("WallMiddle", new TileConstruct(Resources.Load<GameObject>("Prefabs/WallMiddle").GetComponent<Tilemap>()));
        tileConstructs.Add("WallEnd", new TileConstruct(Resources.Load<GameObject>("Prefabs/WallEnd").GetComponent<Tilemap>()));
        tileConstructs.Add("WallBMiddle", new TileConstruct(Resources.Load<GameObject>("Prefabs/WallBMiddle").GetComponent<Tilemap>()));
        tilemaps.Add("Ground", groundMap);
        tilemaps.Add("Rails", railMap);
        tilemaps.Add("Walls", wallMap);
        currentGroundTile = tiles["MountainG"];
    }

    private void LoadMap()
    {
        int rand = Random.Range(40, 70);
        SetPlayerStart();
        SetRailWay(rand);
        FillRemains();
        PlaceWalls();
    }

    private void SetPlayerStart()
    {
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                Vector3Int tilePos = new Vector3Int(i, j, 0);
                PlaceTile(currentGroundTile, tilePos, "Ground");
            }
        }
    }

    private void SetRailWay(int length)
    {
        minY = -2;
        maxY = length;
        Vector3Int railPos = new Vector3Int(0, minY, 0);
        for (int i = -2; i < length; i++)
        {
            Vector3Int walkerDir = (Random.Range(0, 2) == 0) ? Vector3Int.right : Vector3Int.left;
            new Walker(railPos, walkerDir, Random.Range(10, 50));
            PlaceRail(railPos);
            railPos += Vector3Int.up;
        }
    }

    private void FillRemains()
    {
        Vector3Int mapSize = groundMap.cellBounds.size;
        for (int i = -(mapSize.x / 2); i < (mapSize.x / 2); i++)
        {
            for (int j = mapSize.y; j > -(mapSize.y / 2); j--)
            {
                Vector3Int tilePos = new Vector3Int(i, j, 0);
                if (groundMap.GetTile(tilePos) == null && CheckTileNeighbors(tilePos))
                {
                    PlaceTile(currentGroundTile, tilePos, "Ground");
                }
            }
        }
    }

    private bool CheckTileNeighbors(Vector3Int tilePos)
    {
        Vector3Int neighborPos = Vector3Int.left;
        for (int i = 0; i < 4; i++)
        {
            if (groundMap.GetTile(tilePos + neighborPos) == null)
            {
                return false;
            }
            neighborPos = TurnVector(neighborPos);
            neighborPos *= (i == 2) ? -1 : 1;
        }
        return true;
    }

    private void PlaceWalls()
    {
        Vector3Int mapSize = groundMap.cellBounds.size;
        for (int i = -(mapSize.x / 2) - 2; i < (mapSize.x / 2) + 2; i++)
        {
            for (int j = mapSize.y; j > -(mapSize.y / 2); j--)
            {
                Vector3Int tilePos = new Vector3Int(i, j, 0);
                if (groundMap.GetTile(tilePos) != null && groundMap.GetTile(tilePos + Vector3Int.up) == null)
                {
                    tileConstructs["WallMiddle"].PlaceConstruct(tilePos);
                    break;
                }
            }
        }
        for (int i = -(mapSize.x / 2) - 2; i < (mapSize.x / 2) + 2; i++)
        {
            for (int j = -mapSize.y; j < (mapSize.y / 2); j++)
            {
                Vector3Int tilePos = new Vector3Int(i, j, 0);
                if (groundMap.GetTile(tilePos) != null && groundMap.GetTile(tilePos + Vector3Int.down) == null)
                {
                    tileConstructs["WallBMiddle"].PlaceConstruct(tilePos);
                    break;
                }
            }
        }
    }

    private void PlaceRail(Vector3Int pos)
    {
        railMap.SetTile(pos, tiles["Rail"]);
        for (int i = -1; i < 2; i++)
        {
            Vector3Int groundPos = pos + new Vector3Int(i, 0, 0);
            PlaceTile(tiles["MountainG"], groundPos, "Ground");
        }
    }

    public void PlaceTile(TileBase tileType, Vector3Int pos, string mapName)
    {
        if (tilemaps.ContainsKey(mapName))
        {
            tilemaps[mapName].SetTile(pos, tileType);
        }
    }

    public Vector3Int TurnVector(Vector3Int vec)
    {
        Vector3Int turnedVec = new Vector3Int(vec.y, vec.x, 0);
        return turnedVec;
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
        tiles = new List<TileAndPos>();
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
            GenerationManager.Instance.PlaceTile(tile.tile, tilePos, "Walls");
        }
    }
}

public class Walker
{
    protected Vector3Int start;
    protected Vector3Int dir;
    protected int length;
    protected int turnChance;

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
        turnChance = 5;
        for (int i = 0; i < length; i++)
        {
            if (!CheckYPos(pos + dir))
            {
                Turn();
            }
            pos += dir;
            PlaceGround(pos);
            int shouldTurn = Random.Range(0, turnChance);
            if (shouldTurn == 0)
            {
                Turn();
            }
            else
            {
                turnChance--;
            }
        }
    }

    protected void Turn()
    {
        turnChance = 10;
        dir = ChangeDirection();
    }

    protected bool CheckYPos(Vector3Int curPos)
    {
        bool isYWithinBounds = (curPos.y >= GenerationManager.minY && curPos.y <= GenerationManager.maxY) ? true : false;
        return isYWithinBounds;
    }

    protected void PlaceGround(Vector3Int pos)
    {
        Vector3Int groundDir = GenerationManager.Instance.TurnVector(dir);
        for (int i = -1; i < 2; i++)
        {
            Vector3Int groundPos = pos + (groundDir * i);
            GenerationManager.Instance.PlaceTile(GenerationManager.Instance.currentGroundTile, groundPos, "Ground");
        }
    }

    protected Vector3Int ChangeDirection()
    {
        Vector3Int newDir = GenerationManager.Instance.TurnVector(dir);
        int shouldReverseDir = Random.Range(0, 2);
        int x = (shouldReverseDir == 0) ? newDir.x : -newDir.x;
        int y = (shouldReverseDir == 0) ? newDir.y : -newDir.y;
        newDir = new Vector3Int(x, y, 0);
        return newDir;

    }
}

public class RailPartWalker : Walker
{
    public RailPartWalker(Vector3Int _start, Vector3Int _dir, int _length) : base(_start, _dir, _length)
    {

    }
}