using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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

    public Text descr;
    public Text scoreTxt;

    public int startX, startY;

    public List<GameObject> btn = new List<GameObject>();
    public GameObject buildingsUI;
    public GameObject buildingsList;

    public GameObject woodUI, stoneUI, goldUI;
    private int wood, stone, gold;
    private int score;
    private int maxStorage;
    private int extraRes;
    private bool hasAcademy;
    private bool hasWonder;

    private bool gameOver;

    public Sprite[] buildingSprites;

    //endscreen
    public GameObject endScreen;
    public Text winText;
    public Text scr;
    public Text highScore;

    private string highScoreTxt;

    private bool save = true;
    private bool isHighScore = false;

    //audio
    private AudioSource audioSrc;
    public AudioClip[] sfx;
    private bool playOnce = true;

    // Use this for initialization
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        //audioSrc.clip = music;
        //audioSrc.loop = true;
        //audioSrc.Play();

        gameOver = false;

        //create level generator
        levelGen = Instantiate(levelGenPrefab);
        levelScript = levelGen.GetComponent<scr_LevelGenerator>(); //levelScript.level;
        levelScript.gameManager = this.gameObject;
        levelScript.cam = cam;

        level = levelScript.level;

        CreateSaveFile();

        score = 0;

        maxStorage = 5000;
        extraRes = 0;

        hasAcademy = false;
        hasWonder = false;

        wood = 5000;
        stone = 5000;
        gold = 5000;

        Invoke("PlaceTownhall", 0.1f);

        //load highscore
        highScoreTxt = LoadHighscore();
    }

    private void PlaceTownhall()
    {
        levelScript.levelObjects[startX, startY].GetComponent<scr_Tile>().isBuilding = true;
        levelScript.levelObjects[startX, startY].GetComponent<scr_Tile>().isTownhall = true;
        levelScript.levelObjects[startX, startY].GetComponent<scr_Tile>().SetBuilding(scr_Tile.buildings.townhall);
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(startX + " " + startY);
        //Debug.Log(levelScript.levelObjects[startX, startY].GetComponent<scr_Tile>().x + " " + levelScript.levelObjects[startX, startY].GetComponent<scr_Tile>().y);

        if (!gameOver)
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
            scoreTxt.text = "" + score;
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

                switch (scrTile.buildingType)
                {
                    case scr_Tile.buildings.lumbercamp:
                        if (playOnce)
                        {
                            audioSrc.clip = sfx[6];
                            audioSrc.loop = false;
                            audioSrc.Play();
                            playOnce = false;
                        }
                        else
                        {
                            Invoke("ResetAudio", 1);
                        }
                        break;
                    case scr_Tile.buildings.miningcamp1:
                        if (playOnce)
                        {
                            audioSrc.clip = sfx[9];
                            audioSrc.loop = false;
                            audioSrc.Play();
                            playOnce = false;
                        }
                        else
                        {
                            Invoke("ResetAudio", 1);
                        }
                        break;
                    case scr_Tile.buildings.miningcamp2:
                        if (playOnce)
                        {
                            audioSrc.clip = sfx[9];
                            audioSrc.loop = false;
                            audioSrc.Play();
                            playOnce = false;
                        }
                        else
                        {
                            Invoke("ResetAudio", 1);
                        }
                        break;
                    case scr_Tile.buildings.storehouse:
                        if (playOnce)
                        {
                            audioSrc.clip = sfx[3];
                            audioSrc.loop = false;
                            audioSrc.Play();
                            playOnce = false;
                        }
                        else
                        {
                            Invoke("ResetAudio", 1);
                        }
                        break;
                    case scr_Tile.buildings.dock:
                        if (playOnce)
                        {
                            audioSrc.clip = sfx[2];
                            audioSrc.loop = false;
                            audioSrc.Play();
                            playOnce = false;
                        }
                        else
                        {
                            Invoke("ResetAudio", 1);
                        }
                        break;
                    case scr_Tile.buildings.farm:
                        if (playOnce)
                        {
                            audioSrc.clip = sfx[0];
                            audioSrc.loop = false;
                            audioSrc.Play();
                            playOnce = false;
                        }
                        else
                        {
                            Invoke("ResetAudio", 1);
                        }
                        break;
                    case scr_Tile.buildings.temple:
                        if (playOnce)
                        {
                            audioSrc.clip = sfx[8];
                            audioSrc.loop = false;
                            audioSrc.Play();
                            playOnce = false;
                        }
                        else
                        {
                            Invoke("ResetAudio", 2);
                        }
                        break;
                    case scr_Tile.buildings.market:
                        if (playOnce)
                        {
                            audioSrc.clip = sfx[3];
                            audioSrc.loop = false;
                            audioSrc.Play();
                            playOnce = false;
                        }
                        else
                        {
                            Invoke("ResetAudio", 1);
                        }
                        break;
                    case scr_Tile.buildings.wonder:
                        if (playOnce)
                        {
                            audioSrc.clip = sfx[8];
                            audioSrc.loop = false;
                            audioSrc.Play();
                            playOnce = false;
                        }
                        else
                        {
                            Invoke("ResetAudio", 1);
                        }
                        break;
                    case scr_Tile.buildings.workshop:
                        break;
                    case scr_Tile.buildings.statue:
                        if (playOnce)
                        {
                            audioSrc.clip = sfx[8];
                            audioSrc.loop = false;
                            audioSrc.Play();
                            playOnce = false;
                        }
                        else
                        {
                            Invoke("ResetAudio", 1);
                        }
                        break;
                    case scr_Tile.buildings.academy:
                        if (playOnce)
                        {
                            audioSrc.clip = sfx[8];
                            audioSrc.loop = false;
                            audioSrc.Play();
                            playOnce = false;
                        }
                        else
                        {
                            Invoke("ResetAudio", 1);
                        }
                        break;
                    case scr_Tile.buildings.townhall:
                        break;
                    default:
                        if (playOnce)
                        {
                            audioSrc.clip = sfx[1];
                            audioSrc.loop = false;
                            audioSrc.Play();
                            playOnce = false;
                        }
                        else
                        {
                            Invoke("ResetAudio", 1);
                        }
                        break;
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
        else
        {
            //end game

            levelScript.audioSrc.Stop();

            if (playOnce)
            {
                audioSrc.clip = sfx[12];
                audioSrc.loop = false;
                audioSrc.Play();
                playOnce = false;
            }else
            {
                Invoke("ResetAudio", 5);
            }

            DeselectTile();
            endScreen.SetActive(true);
            if (hasWonder)
            {
                winText.text = "WINNER";
            }
            else
            {
                winText.text = "LOSER";
            }

            scr.text = "" + score;

            int _temp;
            int.TryParse(highScoreTxt, out _temp);

            if (_temp > score)
            {
                highScore.text = "HIGHSCORE: " + highScoreTxt;
            }
            else
            {
                highScore.text = "NEW HIGHSCORE: " + score;
                isHighScore = true;
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Debug.Log("QUIT");
                if (isHighScore)
                {
                    if (save)
                    {
                        SaveHighscore(scr.text);
                        save = false;
                    }
                }
                Application.Quit();
            }

        }
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

    #region buttonFuncs

    //btn functions
    public void Harvest()
    {
        score += 1;
        //Debug.Log("CLICK!");
        switch (scrTile.tileType)
        {
            case scr_Tile.tile.forest:

                wood += 25 + extraRes;
                scrTile.UpdateTile(scr_Tile.tile.floor);

                break;
            case scr_Tile.tile.stone:

                stone += 25 + extraRes;
                scrTile.UpdateTile(scr_Tile.tile.floor);

                break;
            case scr_Tile.tile.gold:

                gold += 25 + extraRes;
                scrTile.UpdateTile(scr_Tile.tile.floor);

                break;
            default:
                break;
        }
        if (playOnce)
        {
            audioSrc.clip = sfx[0];
            audioSrc.loop = false;
            audioSrc.Play();
            playOnce = false;
        }
        else
        {
            Invoke("ResetAudio", 1);
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
                    score += 7;
                    wood -= 50;
                    stone -= 50;
                    gold += 150;
                    scrTile.UpdateTile(scr_Tile.tile.floor);
                    scrTile.SetBuilding(scr_Tile.buildings.dock);

                    if (playOnce)
                    {
                        audioSrc.clip = sfx[2];
                        audioSrc.loop = false;
                        audioSrc.Play();
                        playOnce = false;
                    }
                    else
                    {
                        Invoke("ResetAudio", 1);
                    }

                    DeselectTile();

                }
                break;
            case scr_Tile.tile.forest:
                //lumber
                if (stone - 15 >= 0)
                {
                    score += 5;
                    stone -= 15;
                    wood += 75 + extraRes;
                    scrTile.UpdateTile(scr_Tile.tile.floor);
                    scrTile.SetBuilding(scr_Tile.buildings.lumbercamp);

                    if (playOnce)
                    {
                        audioSrc.clip = sfx[6];
                        audioSrc.loop = false;
                        audioSrc.Play();
                        playOnce = false;
                    }
                    else
                    {
                        Invoke("ResetAudio", 1);
                    }

                    DeselectTile();
                }

                break;
            case scr_Tile.tile.stone:
                //mining

                if (gold - 15 >= 0)
                {
                    score += 5;
                    gold -= 15;
                    stone += 75 + extraRes;
                    scrTile.UpdateTile(scr_Tile.tile.floor);
                    scrTile.SetBuilding(scr_Tile.buildings.miningcamp1);

                    if (playOnce)
                    {
                        audioSrc.clip = sfx[9];
                        audioSrc.loop = false;
                        audioSrc.Play();
                        playOnce = false;
                    }
                    else
                    {
                        Invoke("ResetAudio", 1);
                    }

                    DeselectTile();
                }

                break;
            case scr_Tile.tile.gold:
                //mining

                if (wood - 15 >= 0)
                {
                    score += 5;
                    wood -= 15;
                    gold += 75 + extraRes;
                    scrTile.UpdateTile(scr_Tile.tile.floor);
                    scrTile.SetBuilding(scr_Tile.buildings.miningcamp2);

                    if (playOnce)
                    {
                        audioSrc.clip = sfx[9];
                        audioSrc.loop = false;
                        audioSrc.Play();
                        playOnce = false;
                    }
                    else
                    {
                        Invoke("ResetAudio", 1);
                    }

                    DeselectTile();
                }

                break;
            default:
                break;
        }
    }

    public void ClearTile()
    {

        if (scrTile.isTownhall)
        {
            gameOver = true;
        }
        else
        {
            score -= 5;
            if (scrTile.buildingType != scr_Tile.buildings.dock)
            {
                scrTile.UpdateTile(scr_Tile.tile.floor);
            }
            else
            {
                scrTile.UpdateTile(scr_Tile.tile.empty);
            }

            scrTile.isBuilding = false;
        }
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
            score += 7;
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
            score += 10;
            wood -= 50;
            stone -= 50;
            gold -= 30;
            maxStorage += 100;

            scrTile.UpdateTile(scr_Tile.tile.floor);
            scrTile.SetBuilding(scr_Tile.buildings.storehouse);
            DeselectTile();
        }
    }

    public void BuildMarket()
    {
        if (wood - 25 >= 0 && stone - 25 >= 0)
        {
            score += 25;
            wood -= 50;
            stone -= 50;
            gold += 75;

            scrTile.UpdateTile(scr_Tile.tile.floor);
            scrTile.SetBuilding(scr_Tile.buildings.market);
            DeselectTile();
        }
    }

    public void BuildStatue()
    {
        if (wood - 15 >= 0 && stone - 75 >= 0)
        {
            score += 50;
            wood -= 15;
            stone -= 75;

            scrTile.UpdateTile(scr_Tile.tile.floor);
            scrTile.SetBuilding(scr_Tile.buildings.statue);
            DeselectTile();
        }
    }

    public void BuildTemple()
    {
        if (wood - 50 >= 0 && stone - 75 >= 0 && gold - 150 >= 0)
        {
            score += 100;
            wood -= 50;
            stone -= 75;
            gold -= 150;

            scrTile.UpdateTile(scr_Tile.tile.floor);
            scrTile.SetBuilding(scr_Tile.buildings.temple);
            DeselectTile();
        }
    }

    public void BuildAcademy()
    {
        if (wood - 125 >= 0 && stone - 125 >= 0 && gold - 125 >= 0)
        {
            score += 50;
            wood -= 125;
            stone -= 125;
            gold -= 125;

            hasAcademy = true;

            scrTile.UpdateTile(scr_Tile.tile.floor);
            scrTile.SetBuilding(scr_Tile.buildings.academy);
            DeselectTile();
        }
    }

    public void BuildWorkshop()
    {
        if (hasAcademy)
        {
            if (wood - 75 >= 0 && stone - 75 >= 0 && gold - 75 >= 0)
            {
                score += 25;
                wood -= 75;
                stone -= 75;
                gold -= 75;

                extraRes += 25;

                scrTile.UpdateTile(scr_Tile.tile.floor);
                scrTile.SetBuilding(scr_Tile.buildings.workshop);
                DeselectTile();
            }
        }
    }

    public void BuildWonder()
    {
        if (hasAcademy)
        {
            if (wood - 250 >= 0 && stone - 250 >= 0 && gold - 250 >= 0)
            {
                score += 250;
                wood -= 250;
                stone -= 250;
                gold -= 250;

                hasWonder = true;
                gameOver = true;
                extraRes += 25;

                scrTile.UpdateTile(scr_Tile.tile.floor);
                scrTile.SetBuilding(scr_Tile.buildings.wonder);
                DeselectTile();
            }
        }
    }
    #endregion

    #region mouseHover
    public void MouseHoverHarvest()
    {
        descr.text = "Harvest Resource";
    }

    public void MouseHoverLumber()
    {
        descr.text = ("Build LumberMill: +75w -15s ");
    }

    public void MouseHoverMining1()
    {
        descr.text = ("Build Stone Mining Camp: +75s -15g ");
    }

    public void MouseHoverMining2()
    {
        descr.text = ("Build Gold Mining Camp: +75g -15w ");
    }

    public void MouseHoverStorehouse()
    {
        descr.text = ("Build Storehouse: -50w -50s -30g +100Storage ");
    }

    public void MouseHoverDock()
    {
        descr.text = ("Build Dock: +125g -25w -25s");
    }

    public void MouseHoverFarm()
    {
        descr.text = ("Build Farm: +30w -5g");
    }

    public void MouseHoverTemple()
    {
        descr.text = ("Build Temple: -150g -50w -75s +BigScore Bonus");
    }

    public void MouseHoverMarket()
    {
        descr.text = ("Build Market: -50w -50s +75g");
    }

    public void MouseHoverWonder()
    {
        descr.text = ("Build Wonder: -250w -250s 250g +WinGame");
    }

    public void MouseHoverWorkshop()
    {
        descr.text = ("Build Workshop: -75w -75s -75g +Res Harvest");
    }

    public void MouseHoverStatue()
    {
        descr.text = ("Build Statue: -15w -75s +Score Bonus");
    }

    public void MouseHoverAcademy()
    {
        descr.text = ("Build Academy: -125w -125s -125g +Unlock Workshop & Wonder");
    }

    public void MouseHoverExit()
    {
        descr.text = "";
    }

    #endregion

    private void CreateSaveFile()
    {
        string path = Application.dataPath + "/Save/Highscore.txt";

        if (!File.Exists(path))
        {
            StreamWriter writer = new StreamWriter(path, true);
            writer.Close();
        }
    }

    private void SaveHighscore(string score)
    {
        string path = Application.dataPath + "/Save/Highscore.txt";

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(score);
        writer.Close();
    }

    private string LoadHighscore()
    {
        string hs = "";
        string line;
        string path = Application.dataPath + "/Save/Highscore.txt";

        StreamReader reader = new StreamReader(path);

        do
        {
            line = reader.ReadLine();
            if (line != null)
            {
                hs = line;
            }
        } while (line != null);

        reader.Close();

        return hs;
    }

    private void ResetAudio()
    {
        playOnce = true;
    }

}
