using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_GameManager : MonoBehaviour
{

    public GameObject levelGen;
    public scr_LevelGenerator levelScript;
    public int[,] level;

    public GameObject selectedTile;

    public Text text;
    public int wood, stone, gold;

    // Use this for initialization
    void Start()
    {
        level = levelScript.level;
    }

    // Update is called once per frame
    void Update()
    {
        //text.text = "W: " + wood + " S: " + stone + " G: " + gold;

        //select tile
        if (Input.GetMouseButtonDown(0) && (Input.mousePosition.x<756f))
        {
            Debug.Log(Input.mousePosition.x + " :: " + Input.mousePosition.y);                                                                              //posicao no ecra
            //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition).x + " :: " + Camera.main.ScreenToWorldPoint(Input.mousePosition).y);            //posicao no mundo

            selectedTile = selected(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y));
            //selectedTile.
        }
    }
    
    public GameObject selected(Vector2 pos)
    {
        var obj = Physics2D.OverlapPoint(pos);
        return obj.gameObject;
    }
}
