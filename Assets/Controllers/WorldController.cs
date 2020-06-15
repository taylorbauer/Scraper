﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldController : MonoBehaviour
{

    public static WorldController instance {get; protected set;}

    public Sprite floorSprite;
    public World world {get; protected set;}
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null) {
            Debug.LogError("Problem!  There should never be 2 WorldControllers.");
        }
        instance = this;

        world = new World();
        //world.RandomizeTiles();

        // Create GameObject for each tiles, so we can see them
        for (int x = 0; x < world.width; x ++) {
            for (int y = 0; y < world.height; y++) {
                Tile tile_data = world.GetTileAt(x,y,0);
                GameObject tile_go = new GameObject();
                tile_go.gameObject.name = "Tile_" + x + "_" + y + "_0";
                tile_go.AddComponent<SpriteRenderer>();  // Empty SpriteRenderer for now
                tile_go.transform.position = new Vector3( tile_data.x, tile_data.y, 0);
                tile_data.registerTileTypeChangedCallback( (tile) => { OnTileTypeChanged(tile, tile_go); } );
                OnTileTypeChanged(tile_data, tile_go);
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

    void OnTileTypeChanged(Tile tile_data, GameObject tile_go) {
        if(tile_data.type == Tile.TileType.Floor) {
            tile_go.GetComponent<SpriteRenderer>().sprite = floorSprite;
        }  
        else if (tile_data.type == Tile.TileType.Empty){
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
}
