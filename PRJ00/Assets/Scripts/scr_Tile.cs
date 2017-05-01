using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Tile : MonoBehaviour {

    public enum tile { empty, floor, forest, stone, gold };

    public GameObject levelGen;
    public scr_LevelGenerator levelScript;
    public int[,] level;

    public int x, y;
    public tile tileType;

    public Sprite[] tileSprites;

    public SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start () {

        spriteRenderer = GetComponent<SpriteRenderer>();

        levelGen = GameObject.Find("Level");
        levelScript = levelGen.GetComponent<scr_LevelGenerator>();
        level = levelScript.level;

        //set sprite
        switch (tileType)
        {
            case tile.empty:
                spriteRenderer.sprite = tileSprites[0];
                break;
            case tile.floor:
                spriteRenderer.sprite = tileSprites[1];
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
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
