using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{

    //Probably delete all this eventually
    // Camera tutorial at https://www.youtube.com/watch?v=txzXMdzFiYw
    public GameObject circleCursorPrefab;
    public float scrollIntensity;
    public float zoomOutMax;
    public float zoomOutMin;
    bool buildModeIsObjects = false;
    string buildModeObjectType;
    Vector3 lastFramePosition;
    Vector3 dragStartPosition;
    Vector3 currentFramePosition;
    List<GameObject> dragPreviewGameObjects;

    TileType buildModeTile = TileType.Floor;

    // Start is called before the first frame update
    void Start()
    {
        dragPreviewGameObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        currentFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentFramePosition.z = 0;

        // Update circle cursor
        //UpdateCursor();
        UpdateDragging();
        UpdateCameraMovement();


        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;
    }
    // void UpdateCursor() {
    //     Tile tileUnderMouse = WorldController.instance.getTileAtWorldCoord(currentFramePosition);
    //     if (tileUnderMouse != null) {
    //         circleCursor.SetActive(true);
    //         Vector3 cursorPosition = new Vector3(tileUnderMouse.x, tileUnderMouse.y, 0);
    //         circleCursor.transform.position = cursorPosition;
    //     }
    //     else {
    //         circleCursor.SetActive(false);
    //     }
    // }
    void UpdateDragging()
    {
        // Handling left mouse clicks

        // If we are over a UI element, we need to bail out
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // ^^Like, specifically, is it over a user interface object.
            return;
        }

        // Starting drag
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPosition = currentFramePosition;
        }

        int start_x = Mathf.FloorToInt(dragStartPosition.x);
        int end_x = Mathf.FloorToInt(currentFramePosition.x);
        if (end_x < start_x)
        { //swappy do
            int temp = end_x;
            end_x = start_x;
            start_x = temp;
        }
        int start_y = Mathf.FloorToInt(dragStartPosition.y);
        int end_y = Mathf.FloorToInt(currentFramePosition.y);
        if (end_y < start_y)
        { //swappy do
            int temp = end_y;
            end_y = start_y;
            start_y = temp;
        }

        // Clean up old drag previews
        while (dragPreviewGameObjects.Count > 0)
        {
            GameObject go = dragPreviewGameObjects[0];
            dragPreviewGameObjects.RemoveAt(0);
            Destroy(go);
        }

        if (Input.GetMouseButton(0))
        {  // If the mouse button is currently held down
            // May need to implement pooling here, it is how Quill18 did it, he provided
            // a pooling script that he wrote to reduce work each frame
            // instead of destroying and recreating each cursor each frame,
            // it adds them all to an array and disables/enables them if they are required
            // Info for this at https://www.youtube.com/watch?v=9fDAuZH0_hE
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.instance.world.GetTileAt(x, y, 0);

                    if (t != null)
                    {
                        // Display the building hint on top of this tile position
                        GameObject go = (GameObject)Instantiate(circleCursorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        go.transform.SetParent(this.transform, true);
                        dragPreviewGameObjects.Add(go);
                    }
                }
            }
        }

        // Ending drag
        if (Input.GetMouseButtonUp(0))
        {
            for (int x = start_x; x <= end_x; x++)
            {
                for (int y = start_y; y <= end_y; y++)
                {
                    Tile t = WorldController.instance.world.GetTileAt(x, y, 0);
                    if (t != null)
                    {
                        if (buildModeIsObjects)
                        {
                            // Create the furniture and assign it to the tile
                            WorldController.instance.world.PlaceFurniture(buildModeObjectType, t);
                        }
                        else
                        {
                            t.type = buildModeTile;
                        }
                    }
                }
            }
        }
    }

    void UpdateCameraMovement()
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {// 0 is left, 1 is right, 2 is middle

            Vector3 diff = lastFramePosition - currentFramePosition;
            Camera.main.transform.Translate(diff);
            // Debug.Log("Moved camera" + diff + " units");
        }

        // Zoom stuff
        Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel") * scrollIntensity;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, zoomOutMin, zoomOutMax);
    }

    public void SetMode_BuildFloor()
    {
        buildModeIsObjects = false;
        buildModeTile = TileType.Floor;
    }
    public void SetMode_Bulldoze()
    {
        buildModeIsObjects = false;
        buildModeTile = TileType.Empty;
    }

    public void SetMode_BuildFurniture(string objectType)
    { // Wall isn't a tile, it's an Furniture that exists on TOP of a tile
        buildModeIsObjects = true;
        buildModeObjectType = objectType;
    }

}
