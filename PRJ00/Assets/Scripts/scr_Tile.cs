using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Tile : MonoBehaviour
{

    /*
     build//
        road;
        lumber camp; +75w -15s
        mining camp; +75s -15g
        mining camp gold; +75g -15w
        storehouse; store more resources
        dock; +125g -25w -25s
        farm; +50w -5g
        temple; -150g -50w -75s
        market; trade stuff
        wonder; 250w 250s 250g
        workshop; harvest, lumbercamp, miningcamp +25res
        statue; -75s -15w
        academy; unlock workshop, wonder; 
     */

    public enum tile { empty, floor, forest, stone, gold, building };
    public enum buildings { none = -1, lumbercamp, miningcamp1, miningcamp2, storehouse, dock, farm, temple, market, wonder, workshop, statue, academy, townhall };

    public GameObject gameManager;
    public scr_GameManager gmScript;

    public GameObject levelGen;
    public scr_LevelGenerator levelScript;
    public int[,] level;

    public int x, y;
    public tile tileType;
    public buildings buildingType;
    public bool isBuilding;
    public bool isTownhall;

    //guardar as sprites no GM
    public Sprite[] tileEdge;
    public Sprite[] tileSea;
    public Sprite[] tileDirt;
    public Sprite[] tileTree;

    public Sprite tileStone;
    public Sprite tileGold;
    public Sprite tileNULL;

    public SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {


        buildingType = buildings.none;
        isBuilding = false;

        spriteRenderer = GetComponent<SpriteRenderer>();

        gameManager = GameObject.Find("GameManager");
        gmScript = gameManager.GetComponent<scr_GameManager>();

        levelGen = gameManager.GetComponent<scr_GameManager>().levelGen;
        levelScript = levelGen.GetComponent<scr_LevelGenerator>();
        level = levelScript.level;


        //set sprite
        int _i;
        switch (tileType)
        {
            case tile.empty:

                _i = 0;
                if ((int)Random.Range(0,10) == 0)
                {
                    _i = (int)Random.Range(1, tileSea.Length);
                    spriteRenderer.sprite = tileSea[_i];
                }
                else
                {
                    spriteRenderer.sprite = tileSea[0];
                }

                break;
            case tile.floor:

                _i = 0;
                if ((int)Random.Range(0, 10) == 0)
                {
                    _i = (int)Random.Range(0, tileDirt.Length);
                    spriteRenderer.sprite = tileDirt[_i];
                }
                else
                {
                    spriteRenderer.sprite = tileDirt[0];
                }

                break;
            case tile.forest:

                _i = (int)Random.Range(0, tileTree.Length);
                spriteRenderer.sprite = tileTree[_i];

                break;
            case tile.stone:
                spriteRenderer.sprite = tileStone;
                break;
            case tile.gold:
                spriteRenderer.sprite = tileGold;
                break;
            default:
                spriteRenderer.sprite = tileNULL;
                break;
        }

        GameObject go;

        #region SeaEdge
        //check Sea Edge
        if (level[x, y] == 0)
        {
            if (x!=levelScript.gridX-1)
            {
                if (level[x + 1, y] != 0)
                {
                    go = new GameObject("edge_Right");
                    go.AddComponent<SpriteRenderer>().sprite = tileEdge[0];
                    go.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    go.transform.position = transform.position;
                    go.transform.parent = transform;
                }
            }

            if (x != 0)
            {
                if (level[x - 1, y] != 0)
                {
                    go = new GameObject("edge_Left");
                    go.AddComponent<SpriteRenderer>().sprite = tileEdge[1];
                    go.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    go.transform.position = transform.position;
                    go.transform.parent = transform;
                }
            }

            if (y != levelScript.gridY-1)
            {
                if (level[x, y + 1] != 0)
                {
                    go = new GameObject("edge_Top");
                    go.AddComponent<SpriteRenderer>().sprite = tileEdge[2];
                    go.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    go.transform.position = transform.position;
                    go.transform.parent = transform;
                }
            }

            if (y != 0)
            {
                if (level[x, y - 1] != 0)
                {
                    go = new GameObject("edge_Bottom");
                    go.AddComponent<SpriteRenderer>().sprite = tileEdge[3];
                    go.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    go.transform.position = transform.position;
                    go.transform.parent = transform;
                }
            }
        }
        #endregion

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateTile(tile newType)
    {
        switch (newType)
        {
            case tile.empty:

                tileType = newType;
                spriteRenderer.sprite = tileSea[0];

                break;
            case tile.floor:

                tileType = newType;
                spriteRenderer.sprite = tileDirt[0];

                break;
            default:
                break;
        }
    }

    public void SetBuilding(buildings newBuilding)
    {
        if (tileType == tile.floor)
        {
            isBuilding = true;
            buildingType = newBuilding;
            spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];
            #region comment
            /*
            switch (newBuilding)
            {
                case buildings.lumbercamp:

                    buildingType = newBuilding;
                    spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];

                    break;
                case buildings.miningcamp1:

                    buildingType = newBuilding;
                    spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];

                    break;
                case buildings.miningcamp2:

                    buildingType = newBuilding;
                    spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];

                    break;
                case buildings.storehouse:

                    buildingType = newBuilding;
                    spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];

                    break;
                case buildings.dock:

                    buildingType = newBuilding;
                    spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];

                    break;
                case buildings.farm:

                    buildingType = newBuilding;
                    spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];

                    break;
                case buildings.temple:

                    buildingType = newBuilding;
                    spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];

                    break;
                case buildings.market:

                    buildingType = newBuilding;
                    spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];

                    break;
                case buildings.wonder:

                    buildingType = newBuilding;
                    spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];

                    break;
                case buildings.workshop:

                    buildingType = newBuilding;
                    spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];

                    break;
                case buildings.statue:

                    buildingType = newBuilding;
                    spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];

                    break;
                case buildings.academy:

                    buildingType = newBuilding;
                    spriteRenderer.sprite = gmScript.buildingSprites[(int)newBuilding];

                    break;
                default:
                    break;
            }
            */
            #endregion
        }
    }
}
