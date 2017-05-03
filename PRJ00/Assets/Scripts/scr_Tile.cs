using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Tile : MonoBehaviour
{

    public enum tile { empty, floor, forest, stone, gold };

    public GameObject gameManager;

    public GameObject levelGen;
    public scr_LevelGenerator levelScript;
    public int[,] level;

    public int x, y;
    public tile tileType;

    public Sprite[] tileEdge;
    public Sprite[] tileSea;

    public Sprite[] tileDirt;

    public Sprite[] tileSprites;

    public SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();

        gameManager = GameObject.Find("GameManager");

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
                    _i = (int)Random.Range(1, tileDirt.Length);
                    spriteRenderer.sprite = tileDirt[_i];
                }
                else
                {
                    spriteRenderer.sprite = tileDirt[0];
                }

                break;
            case tile.forest:
                spriteRenderer.sprite = tileSprites[3];
                break;
            case tile.stone:
                spriteRenderer.sprite = tileSprites[4];
                break;
            case tile.gold:
                spriteRenderer.sprite = tileSprites[2];
                break;
            default:
                spriteRenderer.sprite = tileSprites[6];
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
}
