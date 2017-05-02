using UnityEngine;
using System.Collections;

public class scr_LevelGenerator : MonoBehaviour
{
    public enum tile { empty, floor, forest, stone, gold };

    public GameObject gameManager;
    public GameObject cam;

    public int steps;
    public int gridX, gridY;

    public GameObject newTile;      //test
    //public GameObject[] tileTypes;

    public int forestRate;
    public int stoneRate;
    public int goldRate;

    public int[,] level;
    public GameObject[,] levelObjects;


    private int startX, startY;

    private int floorCount = 0;

    // Use this for initialization
    void Start()
    {

        level = new int[gridX, gridY];
        levelObjects = new GameObject[gridX, gridY];

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                level[x, y] = (int)tile.empty;
            }
        }

        int cX = (int)Random.Range(5, gridX - 5);
        int cY = (int)Random.Range(5, gridX - 5);
        int cDir = (int)Random.Range(0, 4);

        int odds = 1;

        for (int i = 0; i < steps; i++)
        {
            level[cX, cY] = (int)tile.floor;

            if ((int)Random.Range(0, odds + 1) == odds)
            {
                cDir = (int)Random.Range(0, 4);
            }

            switch (cDir)
            {
                case 0:
                    if (cX - 1 > 1)
                    {
                        cX -= 1;
                    }
                    break;
                case 1:
                    if (cY + 1 < gridY - 1)
                    {
                        cY += 1;
                    }
                    break;
                case 2:
                    if (cX + 1 < gridX - 1)
                    {
                        cX += 1;
                    }
                    break;
                case 3:
                    if (cY - 1 > 1)
                    {
                        cY -= 1;
                    }
                    break;
            }
        }

        foreach (int item in level)
        {
            if (item == (int)tile.floor)
            {
                floorCount++;
            }
        }

        #region setRes
        //forest
        float temp = (float)Random.Range(forestRate - 10, forestRate) / 100;
        int forestQtd = Mathf.FloorToInt(floorCount * temp);
        do
        {
            int rndX = (int)Random.Range(0, 64);
            int rndY = (int)Random.Range(0, 64);

            if (level[rndX, rndY] != (int)tile.empty)
            {
                level[rndX, rndY] = (int)tile.forest;
                forestQtd--;
            }
        } while (forestQtd > 0);

        //stone
        temp = (float)Random.Range(stoneRate - 10, stoneRate) / 100;
        int stoneQtd = Mathf.FloorToInt(floorCount * temp);
        do
        {
            int rndX = (int)Random.Range(0, 64);
            int rndY = (int)Random.Range(0, 64);

            if (level[rndX, rndY] != (int)tile.empty)
            {
                level[rndX, rndY] = (int)tile.stone;
                stoneQtd--;
            }
        } while (stoneQtd > 0);

        //gold
        temp = 0.07f;
        int goldQtd = Mathf.FloorToInt(floorCount * temp);
        do
        {
            int rndX = (int)Random.Range(0, 64);
            int rndY = (int)Random.Range(0, 64);

            if (level[rndX, rndY] != (int)tile.empty)
            {
                level[rndX, rndY] = (int)tile.gold;
                goldQtd--;
            }
        } while (goldQtd > 0);


        //holes
        for (int x = 1; x < gridX - 2; x++)
        {
            for (int y = 1; y < gridY - 2; y++)
            {
                if (level[x, y] == (int)tile.empty)
                {
                    if (level[x + 1, y] != (int)tile.empty && level[x - 1, y] != (int)tile.empty && level[x, y + 1] != (int)tile.empty && level[x, y - 1] != (int)tile.empty)
                    {
                        level[x, y] = (int)tile.floor;
                        //Debug.Log("x: "+x + "y: "+y);
                    }
                }

                if (level[x, y] == (int)tile.empty && level[x + 1, y] == (int)tile.empty)
                {
                    if (level[x + 2, y] != (int)tile.empty && level[x - 1, y] != (int)tile.empty && level[x, y + 1] != (int)tile.empty && level[x + 1, y + 1] != (int)tile.empty && level[x, y - 1] != (int)tile.empty && level[x + 1, y - 1] != (int)tile.empty)
                    {
                        level[x, y] = (int)tile.floor;
                        level[x + 1, y] = (int)tile.floor;
                    }
                }

                if (level[x, y] == (int)tile.empty && level[x, y + 1] == (int)tile.empty)
                {
                    if (level[x, y - 1] != (int)tile.empty && level[x, y + 2] != (int)tile.empty && level[x + 1, y] != (int)tile.empty && level[x - 1, y] != (int)tile.empty && level[x + 1, y + 1] != (int)tile.empty && level[x - 1, y + 1] != (int)tile.empty)
                    {
                        level[x, y] = (int)tile.floor;
                        level[x + 1, y] = (int)tile.floor;
                    }
                }
            }
        }
        #endregion

        #region instantiate
        //inst
        GameObject go;

        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                if (level[x, y] == (int)tile.empty)
                {
                    go = Instantiate(newTile, new Vector3(x + 64, y + 64, 0), Quaternion.identity);
                    go.transform.parent = transform;

                    go.GetComponent<scr_Tile>().x = x;
                    go.GetComponent<scr_Tile>().y = y;
                    go.GetComponent<scr_Tile>().tileType = (scr_Tile.tile)level[x, y];

                    //go = Instantiate(tileTypes[(int)tile.empty], new Vector3(x + 64, y + 64, 0), Quaternion.identity);
                    //go.transform.parent = transform;

                    levelObjects[x, y] = go;
                }
                else if (level[x, y] == (int)tile.floor)
                {
                    go = Instantiate(newTile, new Vector3(x + 64, y + 64, 0), Quaternion.identity);
                    go.transform.parent = transform;

                    go.GetComponent<scr_Tile>().x = x;
                    go.GetComponent<scr_Tile>().y = y;
                    go.GetComponent<scr_Tile>().tileType = (scr_Tile.tile)level[x, y];

                    //go = Instantiate(tileTypes[(int)tile.floor], new Vector3(x + 64, y + 64, 0), Quaternion.identity);
                    //go.transform.parent = transform;

                    levelObjects[x, y] = go;
                }
                else if (level[x, y] == (int)tile.forest)
                {
                    go = Instantiate(newTile, new Vector3(x + 64, y + 64, 0), Quaternion.identity);
                    go.transform.parent = transform;

                    go.GetComponent<scr_Tile>().x = x;
                    go.GetComponent<scr_Tile>().y = y;
                    go.GetComponent<scr_Tile>().tileType = (scr_Tile.tile)level[x, y];

                    //go = Instantiate(tileTypes[(int)tile.forest], new Vector3(x + 64, y + 64, 0), Quaternion.identity);
                    //go.transform.parent = transform;

                    levelObjects[x, y] = go;
                }
                else if (level[x, y] == (int)tile.stone)
                {
                    go = Instantiate(newTile, new Vector3(x + 64, y + 64, 0), Quaternion.identity);
                    go.transform.parent = transform;

                    go.GetComponent<scr_Tile>().x = x;
                    go.GetComponent<scr_Tile>().y = y;
                    go.GetComponent<scr_Tile>().tileType = (scr_Tile.tile)level[x, y];

                    //go = Instantiate(tileTypes[(int)tile.stone], new Vector3(x + 64, y + 64, 0), Quaternion.identity);
                    //go.transform.parent = transform;

                    levelObjects[x, y] = go;
                }
                else if (level[x, y] == (int)tile.gold)
                {
                    go = Instantiate(newTile, new Vector3(x + 64, y + 64, 0), Quaternion.identity);
                    go.transform.parent = transform;

                    go.GetComponent<scr_Tile>().x = x;
                    go.GetComponent<scr_Tile>().y = y;
                    go.GetComponent<scr_Tile>().tileType = (scr_Tile.tile)level[x, y];

                    //go = Instantiate(tileTypes[(int)tile.gold], new Vector3(x + 64, y + 64, 0), Quaternion.identity);
                    //go.transform.parent = transform;

                    levelObjects[x, y] = go;
                }
            }
        }
        #endregion

        #region startPosition

        //set random start position
        bool _temp = false;
        do
        {
            int _x = (int)Random.Range(0, gridX);
            int _y = (int)Random.Range(0, gridY);

            if (level[_x, _y] != 0)
            {
                startX = _x;
                startY = _y;
                _temp = true;
            }
        } while (!_temp);

        gameManager.GetComponent<scr_GameManager>().startX = startX;
        gameManager.GetComponent<scr_GameManager>().startY = startY;

        //cam.GetComponent<scr_Cam>().x = startX;
        //cam.GetComponent<scr_Cam>().y = startY;

        cam.transform.position = new Vector3(Mathf.Clamp(startX + 64 + 0.5f, 11 + 0.5f + 64, 53 - 0.3f + 64), Mathf.Clamp(startY + 64 - 0.5f, 7.5f + 64, 54 + 0.5f + 64), -10);

        #endregion
    }

}
