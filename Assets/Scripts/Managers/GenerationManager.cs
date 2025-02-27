using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerationManager : MonoBehaviour
{
    private static GenerationManager instance;
    public static TestPlayer player;
    public static int maxY;
    public static int minY;
    private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
    private Dictionary<string, TileConstruct> tileConstructs = new Dictionary<string, TileConstruct>();
    private Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();
    private GameObject worldTilemap;
    private Tilemap groundMap;
    private Tilemap railMap;
    private Tilemap wallMap;
    private GameObject railPickup;
    private GameObject endOfRoomOb;
    private GameObject playerOb;
    private GameObject cartOb;
    private CartScript cartScript;
    private GameObject cartEndOb;
    private GameObject screenTransitionOb;
    private List<Transform> exits = new List<Transform>();
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
        player = FindObjectOfType<TestPlayer>().GetComponent<TestPlayer>();
        worldTilemap = GameObject.FindGameObjectWithTag("WorldTilemap");
        groundMap = worldTilemap.transform.GetChild(0).GetComponent<Tilemap>();
        railMap = worldTilemap.transform.GetChild(1).GetComponent<Tilemap>();
        wallMap = worldTilemap.transform.GetChild(2).GetComponent<Tilemap>();
        tiles.Add("Rail", Resources.Load<Tile>("Sprites/Tiles/RailTiles_3"));
        tiles.Add("MountainG", Resources.Load<Tile>("Sprites/Tiles/TestGround"));
        railPickup = Resources.Load<GameObject>("Prefabs/RailPickup");
        endOfRoomOb = Resources.Load<GameObject>("Prefabs/EndOfRoom");
        tileConstructs.Add("WallMiddle", new TileConstruct(Resources.Load<GameObject>("Prefabs/WallMiddle").GetComponent<Tilemap>()));
        tileConstructs.Add("WallEnd", new TileConstruct(Resources.Load<GameObject>("Prefabs/WallEnd").GetComponent<Tilemap>()));
        tileConstructs.Add("WallBMiddle", new TileConstruct(Resources.Load<GameObject>("Prefabs/WallBMiddle").GetComponent<Tilemap>()));
        tileConstructs.Add("EndOfRoom", new TileConstruct(endOfRoomOb.GetComponent<Tilemap>()));
        tilemaps.Add("Ground", groundMap);
        tilemaps.Add("Rails", railMap);
        tilemaps.Add("Walls", wallMap);
        playerOb = player.gameObject;
        cartOb = FindObjectOfType<CartScript>().gameObject;
        cartScript = cartOb.GetComponent<CartScript>();
        cartEndOb = GameObject.FindGameObjectWithTag("CartEnd");
        screenTransitionOb = GameObject.FindGameObjectWithTag("ScreenTransition").transform.GetChild(0).gameObject;
        screenTransitionOb.SetActive(false);
        currentGroundTile = tiles["MountainG"];
        foreach (Transform exit in endOfRoomOb.transform)
        {
            Vector3 exitPos = exit.transform.position + new Vector3(-1, -1, 0);
            Transform newExit = Instantiate(exit, exitPos, Quaternion.identity);
            newExit.gameObject.SetActive(false);
            exits.Add(newExit);
        }
    }

    private void LoadRoomEnd(Vector3Int endPos)
    {
        tileConstructs["EndOfRoom"].PlaceConstruct(endPos);
        foreach (Transform exit in exits)
        {
            Vector3 exitPos = endPos + exit.transform.position;
            exit.position = exitPos;
            exit.gameObject.SetActive(true);
        }
        for (int i = endPos.x - 4; i < endPos.x + 4; i++)
        {
            Vector3Int groundPos = new Vector3Int(i, endPos.y - 1, 0);
            PlaceTile(null, groundPos, "Walls");
        }
    }

    //Implement later for different level types idk :3
    public void LoadNewLevel()
    {
        ClearMap();
        StartCoroutine(SetMoveTransition());
    }

    private IEnumerator SetMoveTransition()
    {
        screenTransitionOb.SetActive(true);
        Animator stAnim = screenTransitionOb.GetComponent<Animator>();
        stAnim.SetTrigger("First");
        yield return new WaitForSeconds(stAnim.GetCurrentAnimatorStateInfo(0).length);
        LoadMap();
        stAnim.SetTrigger("Second");
        yield return new WaitForSeconds(stAnim.GetCurrentAnimatorStateInfo(0).length);
        screenTransitionOb.SetActive(false);
    }

    private void ClearMap()
    {
        groundMap.ClearAllTiles();
        railMap.ClearAllTiles();
        wallMap.ClearAllTiles();
    }

    private void LoadMap()
    {
        int rand = Random.Range(40, 70);
        SetPlayerStart();
        Vector3Int railPos = SetRailWay(rand);
        FillRemains();
        PlaceWalls();
        Vector3Int endRoomPos = railPos + Vector3Int.up + Vector3Int.right;
        LoadRoomEnd(endRoomPos);
    }


    private void SetPlayerStart()
    {
        int size = 4;
        for (int i = -size; i < size + 1; i++)
        {
            for (int j = -size; j < size + 1; j++)
            {
                Vector3Int tilePos = new Vector3Int(i, j, 0);
                PlaceTile(currentGroundTile, tilePos, "Ground");
            }
        }
        cartScript.ResetCart();
        playerOb.transform.position = new Vector3(-1.5f, -1.5f, 0);
        cartOb.transform.position = new Vector3(0, -1.5f, 0);
    }

    private Vector3Int SetRailWay(int length)
    {
        int lostRailChance = 0;
        minY = -4;
        railMap.SetTile(new Vector3Int(0, minY - 1, 0), tiles["Rail"]);
        maxY = length;
        Vector3Int railPos = new Vector3Int(0, minY, 0);
        for (int i = minY; i < length; i++)
        {
            int isRailLost = Random.Range(lostRailChance, length);
            Vector3Int walkerDir = (Random.Range(0, 2) == 0) ? Vector3Int.right : Vector3Int.left;
            if (isRailLost < lostRailChance)
            {
                lostRailChance = -length;
                new RailPartWalker(railPos, walkerDir, Random.Range(150, 180));

            }
            else
            {
                new Walker(railPos, walkerDir, Random.Range(50, 80));
                PlaceRail(railPos);
                lostRailChance++;
            }
            railPos += Vector3Int.up;
        }
        SetCartEnd(railPos + Vector3Int.down);
        return railPos;
    }

    private void SetCartEnd(Vector3Int pos)
    {
        cartEndOb.transform.position = pos;
        cartEndOb.SetActive(true);
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

    public void PlaceRailPickup(Vector3Int railPos)
    {
        Instantiate(railPickup, railPos, Quaternion.identity);
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
    protected Vector3Int pos;
    protected int length;
    protected int turnChance;

    public Walker(Vector3Int _start, Vector3Int _dir, int _length)
    {
        start = _start;
        dir = _dir;
        length = _length;
        Walk();
    }

    protected virtual void Walk()
    {
        pos = start;
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

    protected override void Walk()
    {
        base.Walk();
        PlaceRailItem();
    }

    private void PlaceRailItem()
    {
        for (int i = pos.x - 2; i < pos.x + 3; i++)
        {
            for (int j = pos.y - 2; j < pos.y + 3; j++)
            {
                Vector3Int groundPos = new Vector3Int(i, j, 0);
                PlaceGround(groundPos);
            }
        }
        GenerationManager.Instance.PlaceRailPickup(pos);
    }
}