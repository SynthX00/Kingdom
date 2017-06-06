using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_GameManager : MonoBehaviour
{
    
    public GameObject cam;

    public GameObject levelGenPrefab;

    public GameObject levelGen;
    public scr_LevelGenerator levelScript;
    public int[,] level;

    public GameObject selectedTile;
    public Text selectedTileText;

    public int startX, startY;

    public List<GameObject> btn = new List<GameObject>();

    scr_Tile scrTile;

    // Use this for initialization
    void Start()
    {
        //create level generator
        levelGen = Instantiate(levelGenPrefab);
        levelScript = levelGen.GetComponent<scr_LevelGenerator>(); //levelScript.level;
        levelScript.gameManager = this.gameObject;
        levelScript.cam = cam;

        level = levelScript.level;

    }

    // Update is called once per frame
    void Update()
    {
        //text.text = "W: " + wood + " S: " + stone + " G: " + gold;

        //select tile
        if (Input.GetMouseButtonDown(0) && (Input.mousePosition.x < 756f))
        {
            //Debug.Log(Input.mousePosition.x + " :: " + Input.mousePosition.y                                                                              //posicao no ecra
            //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition).x + " :: " + Camera.main.ScreenToWorldPoint(Input.mousePosition).y);            //posicao no mundo

            selectedTile = selected(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
            scrTile = selectedTile.GetComponent<scr_Tile>();

            selectedTileText.text = "X: " + scrTile.x + " Y: " + scrTile.y + "\t" + "Tile: " + scrTile.tileType;

            //btns
            foreach (GameObject go in btn)
            {
                go.SetActive(false);
            }

            btn[(int)scrTile.tileType].SetActive(true);
        }

        //deselect tile
        if (Input.GetMouseButtonDown(1))
        {
            selectedTile = null;
            selectedTileText.text = "X: \tY: \t Tile: ";
        }

        //start position
        if (Input.GetMouseButtonDown(2))
        {
            selectedTile = null;
            selectedTileText.text = "X: \tY: \t Tile: ";

            cam.transform.position = new Vector3(Mathf.Clamp(startX + 64 + 0.5f, 11 + 0.5f + 64, 53 - 0.3f + 64), Mathf.Clamp(startY + 64 - 0.5f, 7.5f + 64, 54 + 0.5f + 64), -10);
        }
    }

    public GameObject selected(Vector2 pos)
    {
        var obj = Physics2D.OverlapPoint(pos);
        return obj.gameObject;
    }


    //btn trigger
    public void ForestHarvest()
    {
        Debug.Log("");
        Destroy(scrTile.gameObject);
    }
}
