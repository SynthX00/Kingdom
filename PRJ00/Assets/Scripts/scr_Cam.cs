using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Cam : MonoBehaviour
{

    //private GameObject levelObject;
    public scr_LevelGenerator levelScript;
    public int[,] level;

    public float dragSpeed = 1.0f;
    private float currentDragSpeed;
    private Vector3 dragOrigin, pos;
    private bool isPanning = false;

    private int x, y;

    // Use this for initialization
    void Start()
    {

        level = levelScript.level;

        //set camera start position
        //set random x,y
        bool _temp = false;
        do
        {
            int _x = (int)Random.Range(0, levelScript.gridX);
            int _y = (int)Random.Range(0, levelScript.gridY);

            if (level[_x, _y] != 0)
            {
                x = _x;
                y = _y;
                _temp = true;
            }
        } while (!_temp);
        transform.position = new Vector3(Mathf.Clamp(x + 64 + 0.5f, 11 + 0.5f + 64, 53 - 0.3f + 64), Mathf.Clamp(y + 64 - 0.5f, 7.5f + 64, 54 + 0.5f + 64), -10);
    }

    // Update is called once per frame
    void Update(){

        #region MouseDrag

        if (Input.GetMouseButtonDown(1)){
            dragOrigin = Input.mousePosition;
            isPanning = true;
            currentDragSpeed = dragSpeed;
        }

        if (!Input.GetMouseButton(1)){
            isPanning = false;
            if (currentDragSpeed < 0){
                currentDragSpeed += 0.05f;
            }
        }

        if (isPanning){
            pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        }
        

        Vector3 move = new Vector3(pos.x * currentDragSpeed, pos.y * currentDragSpeed, 0);

        Camera.main.transform.Translate(move, Space.Self);

        #endregion

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 11 + 0.5f + 64, 59 - 0.3f + 64), Mathf.Clamp(transform.position.y, 7.5f + 64, 54 + 0.5f + 64), -10);
    }
}
