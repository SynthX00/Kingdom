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
    scr_Tile scrTile;

    public int startX, startY;

    public List<GameObject> btn = new List<GameObject>();
    public GameObject buildingsUI;
    public GameObject buildingsList;

    public GameObject woodUI, stoneUI, goldUI;
    private int wood, stone, gold;

    private int maxStorage;

    public Sprite[] buildingSprites;


    // Use this for initialization
    void Start()
    {
        //create level generator
        levelGen = Instantiate(levelGenPrefab);
        levelScript = levelGen.GetComponent<scr_LevelGenerator>(); //levelScript.level;
        levelScript.gameManager = this.gameObject;
        levelScript.cam = cam;

        level = levelScript.level;

        maxStorage = 200;

        wood = 0;
        stone = 0;
        gold = 0;

    }

    // Update is called once per frame
    void Update()
    {

        #region ui
        //set cap
        if (wood >= maxStorage)
        {
            wood = maxStorage;
            woodUI.GetComponent<Text>().color = new Color(118f / 255f, 95f / 255f, 49f / 255f);//new Color(204,153,51);
        }
        else
        {
            woodUI.GetComponent<Text>().color = new Color(204f / 255f, 153f / 255f, 51f / 255f);
        }

        if (stone >= maxStorage)
        {
            stone = maxStorage;
            stoneUI.GetComponent<Text>().color = new Color(118f / 255f, 95f / 255f, 49f / 255f);
        }
        else
        {
            stoneUI.GetComponent<Text>().color = new Color(204f / 255f, 153f / 255f, 51f / 255f);
        }

        if (gold >= maxStorage)
        {
            gold = maxStorage;
            goldUI.GetComponent<Text>().color = new Color(118f / 255f, 95f / 255f, 49f / 255f);
        }
        else
        {
            goldUI.GetComponent<Text>().color = new Color(204f / 255f, 153f / 255f, 51f / 255f);
        }


        woodUI.GetComponent<Text>().text = "W: " + wood;
        stoneUI.GetComponent<Text>().text = "S: " + stone;
        goldUI.GetComponent<Text>().text = "G: " + gold;
        //text.text = "W: " + wood + " S: " + stone + " G: " + gold;
        #endregion

        #region mouseinput
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
            buildingsUI.SetActive(false);
            buildingsList.SetActive(false);

            if (!scrTile.isBuilding)
            {
                btn[(int)scrTile.tileType].SetActive(true);
            }
            else
            {
                buildingsUI.SetActive(true);
            }
        }

        //deselect tile
        if (Input.GetMouseButtonDown(1))
        {
            DeselectTile();
            //foreach (GameObject go in btn)
            //{
            //    go.SetActive(false);
            //}

            //selectedTile = null;
            //selectedTileText.text = "X: \tY: \t Tile: ";
        }

        //start position
        if (Input.GetMouseButtonDown(2))
        {
            //foreach (GameObject go in btn)
            //{
            //    go.SetActive(false);
            //}

            //selectedTile = null;
            //selectedTileText.text = "X: \tY: \t Tile: ";
            DeselectTile();

            cam.transform.position = new Vector3(Mathf.Clamp(startX + 64 + 0.5f, 11 + 0.5f + 64, 53 - 0.3f + 64), Mathf.Clamp(startY + 64 - 0.5f, 7.5f + 64, 54 + 0.5f + 64), -10);
        }
        #endregion
    }

    public GameObject selected(Vector2 pos)
    {
        var obj = Physics2D.OverlapPoint(pos);
        return obj.gameObject;
    }

    void DeselectTile()
    {
        foreach (GameObject go in btn)
        {
            go.SetActive(false);
        }
        buildingsUI.SetActive(false);
        buildingsList.SetActive(false);

        selectedTile = null;
        selectedTileText.text = "X: \tY: \t Tile: ";
    }

    //btn functions
    public void Harvest()
    {
        //Debug.Log("CLICK!");
        switch (scrTile.tileType)
        {
            case scr_Tile.tile.forest:

                wood += 25;
                scrTile.UpdateTile(scr_Tile.tile.floor);

                break;
            case scr_Tile.tile.stone:

                stone += 25;
                scrTile.UpdateTile(scr_Tile.tile.floor);

                break;
            case scr_Tile.tile.gold:

                gold += 25;
                scrTile.UpdateTile(scr_Tile.tile.floor);

                break;
            default:
                break;
        }

        DeselectTile();
    }

    public void BuildCamp()
    {
        switch (scrTile.tileType)
        {
            case scr_Tile.tile.empty:
                //dock
                if (stone - 50 >= 0 && wood - 50 >= 0)
                {

                    wood -= 50;
                    stone -= 50;
                    gold += 150;
                    scrTile.UpdateTile(scr_Tile.tile.floor);
                    scrTile.SetBuilding(scr_Tile.buildings.dock);
                    DeselectTile();
                }
                break;
            case scr_Tile.tile.forest:
                //lumber
                if (stone - 15 >= 0)
                {

                    stone -= 15;
                    wood += 75;
                    scrTile.UpdateTile(scr_Tile.tile.floor);
                    scrTile.SetBuilding(scr_Tile.buildings.lumbercamp);
                    DeselectTile();
                }

                break;
            case scr_Tile.tile.stone:
                //mining

                if (gold - 15 >= 0)
                {

                    gold -= 15;
                    stone += 75;
                    scrTile.UpdateTile(scr_Tile.tile.floor);
                    scrTile.SetBuilding(scr_Tile.buildings.miningcamp1);
                    DeselectTile();
                }

                break;
            case scr_Tile.tile.gold:
                //mining

                if (wood - 15 >= 0)
                {

                    wood -= 15;
                    gold += 75;
                    scrTile.UpdateTile(scr_Tile.tile.floor);
                    scrTile.SetBuilding(scr_Tile.buildings.miningcamp2);
                    DeselectTile();
                }

                break;
            default:
                break;
        }
    }

    public void ClearTile()
    {
        scrTile.UpdateTile(scr_Tile.tile.floor);
    }

    public void BuildingMenu()
    {
        foreach (GameObject go in btn)
        {
            go.SetActive(false);
        }
        buildingsUI.SetActive(false);


        buildingsList.SetActive(true);
    }

    public void BuildFarm()
    {
        if (gold - 5 >= 0)
        {
            gold -= 5;
            wood += 30;
            scrTile.UpdateTile(scr_Tile.tile.floor);
            scrTile.SetBuilding(scr_Tile.buildings.farm);
            DeselectTile();
        }
    }

    public void BuildStorehouse()
    {
        if (wood - 50 >= 0 && stone - 50 >= 0 && gold - 30 >= 0)
        {
            wood -= 50;
            stone -= 50;
            gold -= 30;
            maxStorage += 100;

            scrTile.UpdateTile(scr_Tile.tile.floor);
            scrTile.SetBuilding(scr_Tile.buildings.storehouse);
            DeselectTile();
        }
    }


}
