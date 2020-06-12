using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    //Probably delete all this eventually
    // Camera tutorial at https://www.youtube.com/watch?v=txzXMdzFiYw
    public GameObject circleCursor;
    Vector3 lastFramePosition;
    Vector3 dragStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentFramePosition.z = 0;

        // Update circle cursor
        Tile tileUnderMouse = getTileAtWorldCoord(currentFramePosition);
        if (tileUnderMouse != null) {
            circleCursor.SetActive(true);
            Vector3 cursorPosition = new Vector3(tileUnderMouse.x, tileUnderMouse.y, 0);
            circleCursor.transform.position = cursorPosition;
        }
        else {
            circleCursor.SetActive(false);
        }

        // Handling left mouse clicks

        // Starting drag
        if (Input.GetMouseButtonDown(0)) {
            dragStartPosition = currentFramePosition;
        }

        // Ending drag
        if (Input.GetMouseButtonUp(0)) {
            int start_x = Mathf.FloorToInt(dragStartPosition.x);
            int end_x =   Mathf.FloorToInt(currentFramePosition.x);
            if (end_x < start_x) { //swappy do
                int temp = end_x;
                end_x = start_x;
                start_x = temp;
            }
            int start_y = Mathf.FloorToInt(dragStartPosition.y);
            int end_y =   Mathf.FloorToInt(currentFramePosition.y);
            if (end_y < start_y) { //swappy do
                int temp = end_y;
                end_y = start_y;
                start_y = temp;
            }
            for(int x = start_x; x <= end_x; x++) {
                for(int y = start_y; y <= end_y; y++) {
                    Tile t = WorldController.instance.world.GetTileAt(x,y,0);
                    if(t != null) {
                        t.type = Tile.TileType.Floor;
                    }
                }
            }
        }

        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) {// 0 is left, 1 is right, 2 is middle
            
            Vector3 diff = lastFramePosition - currentFramePosition;
            Camera.main.transform.Translate(diff);
            // Debug.Log("Moved camera" + diff + " units");
        }

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }

    Tile getTileAtWorldCoord(Vector3 coord) {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return WorldController.instance.world.GetTileAt(x,y, 0);
    }
}
