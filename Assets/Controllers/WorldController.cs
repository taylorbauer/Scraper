using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;


public class WorldController : MonoBehaviour
{

    public static WorldController instance {get; protected set;}
    Dictionary<Tile, GameObject> tileGameObjectMap;
    Dictionary<Furniture, GameObject> furnitureGameObjectMap;
    public Sprite floorSprite; // FIXME
 
    public World world {get; protected set;}

    Dictionary<string, Sprite> furnitureSprites;

    // Start is called before the first frame update
    void Start()
    {
        furnitureSprites = new Dictionary<string, Sprite>();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Furnitures/");

        foreach(Sprite s in sprites) {
            furnitureSprites[s.name] = s;
            Debug.Log("Loaded sprite " + s);
        }

        if (instance != null) {
            Debug.LogError("Problem!  There should never be 2 WorldControllers.");
        }
        instance = this;

        world = new World();

        world.RegisterFurnitureCreated(OnFurnitureCreated);

        //world.RandomizeTiles();
        tileGameObjectMap = new Dictionary<Tile, GameObject>();
        furnitureGameObjectMap = new Dictionary<Furniture, GameObject>();

        // Create GameObject for each tiles, so we can see them
        for (int x = 0; x < world.width; x ++) {
            for (int y = 0; y < world.height; y++) {
                Tile tile_data = world.GetTileAt(x,y,0);
                GameObject tile_go = new GameObject();
                tileGameObjectMap.Add(tile_data, tile_go);
                tile_go.gameObject.name = "Tile_" + x + "_" + y + "_0";
                tile_go.AddComponent<SpriteRenderer>();  // Empty SpriteRenderer for now
                tile_go.transform.position = new Vector3( tile_data.x, tile_data.y, 0);
                tile_data.registerTileTypeChangedCallback( OnTileTypeChanged );
                OnTileTypeChanged(tile_data);
                tile_go.transform.SetParent(this.transform, true);
            }
        }
        //world.RandomizeTiles();
    }

    float randomizeTileTimer = 2f;

    // Update is called once per frame
    void Update()
    {
        // randomizeTileTimer -= Time.deltaTime;

        // if(randomizeTileTimer < 0) {
        //     world.RandomizeTiles();
        //     randomizeTileTimer = 2f;
        // }
    }

    void OnTileTypeChanged(Tile tile_data) {
        if(tileGameObjectMap.ContainsKey(tile_data) == false) {
            Debug.LogError("tileGameObjectMap doesn't contain the tile_data, maybe the tile isn't in the dictionary or the callback is unregistered");
            return;
        }

        GameObject tile_go = tileGameObjectMap[tile_data];

        if (tile_go == null) {
            Debug.LogError("tileGameObjectMap returned null!");
            return;
        }

        if(tile_data.type == TileType.Floor) {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        }  
        else if (tile_data.type == TileType.Empty){
            tile_go.GetComponent<SpriteRenderer>().sprite = null;
        }
        else {
            Debug.Log ("OnTileTypeChanged(): Unrecognized tile type");
        }
    }

    public Tile getTileAtWorldCoord(Vector3 coord) {
        int x = Mathf.FloorToInt(coord.x);
        int y = Mathf.FloorToInt(coord.y);

        return WorldController.instance.world.GetTileAt(x,y, 0);
    }

    public void OnFurnitureCreated( Furniture obj_data ) {
        // Crate a *visual* GameObject linked to this data

        // FIXME: Does not consider multi-tile or rotated objects

        GameObject obj_go = new GameObject();
        furnitureGameObjectMap.Add(obj_data, obj_go);
        obj_go.gameObject.name = obj_data.objectType + "_" + obj_data.tile.x + "_" + obj_data.tile.y + "_0";


        // FIXME: We're assuming that the object must be a wall, so we use the hardcoded reference to the wallsprite
        obj_go.AddComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(obj_data); 
        obj_go.GetComponent<SpriteRenderer>().sortingLayerName = "Furnitures";
        obj_go.transform.position = new Vector3(obj_data.tile.x, obj_data.tile.y, 0);

        obj_data.RegisterOnChangedCallback(OnFurnitureChanged);

        obj_go.transform.SetParent(this.transform, true);
    }

    Sprite GetSpriteForFurniture(Furniture obj) {
        
        if(obj.linksToNeighbor == false) {
            return furnitureSprites[obj.objectType];
        }
        // Otherwise, the sprite name is more coplicated
        string spriteName = obj.objectType + "_";

        int x = obj.tile.x;
        int y = obj.tile.y;

        // Now check for neighbors
        Tile t = world.GetTileAt(x, y + 1, 0);
        if (t != null && t.furniture != null && t.furniture.objectType == obj.objectType) {
            spriteName += "N";
        }
        t = world.GetTileAt(x + 1, y, 0);
        if (t != null && t.furniture != null && t.furniture.objectType == obj.objectType) {
            spriteName += "E";
        }
        t = world.GetTileAt(x, y - 1, 0);
        if (t != null && t.furniture != null && t.furniture.objectType == obj.objectType) {
            spriteName += "S";
        }
        t = world.GetTileAt(x - 1, y, 0);
        if (t != null && t.furniture != null && t.furniture.objectType == obj.objectType) {
            spriteName += "W";
        }

        if (furnitureSprites.ContainsKey(spriteName) == false) {
            Debug.LogError("GetSpriteForFurniture -- No Sprite with name: " + spriteName);
            return null;
        }
        
        return furnitureSprites[spriteName];
    }

    void OnFurnitureChanged(Furniture obj) {
        // Make sure that the furniture's graphics are correct.
        if (furnitureGameObjectMap.ContainsKey(obj) == false) {
            Debug.LogError("OnFurnitureChanged -- That object doesn't seem to exist");
        }

        GameObject obj_go = furnitureGameObjectMap[obj];
        obj_go.GetComponent<SpriteRenderer>().sprite = GetSpriteForFurniture(obj); 
        obj_go.GetComponent<SpriteRenderer>().sortingLayerName = "Furnitures";


    }
}
