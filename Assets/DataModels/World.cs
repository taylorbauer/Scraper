using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class World
{
    Tile[,,] tiles;

    Dictionary<string, Furniture> furniturePrototypes;

    int _width;
    public int width
    {
        get
        {
            return _width;
        }
    }
    int _height;
    public int height
    {
        get
        {
            return _height;
        }
    }
    int _vertical_height;
    int _vertical_depth;

    Action<Furniture> cbFurnitureCreated;

    public World(int width = 150, int height = 150)
    {
        _vertical_height = 0;
        _vertical_depth = 0;
        this._width = width;
        this._height = height;
        tiles = new Tile[width, height, 1];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y, 0] = new Tile(this, x, y, 0);
            }
        }
        RandomizeTiles();
        Debug.Log("World created with " + width * height + " tiles.");

        CreateFurniturePrototypes();


    }

    void CreateFurniturePrototypes()
    {
        Debug.Log("Creating furniture prototypes...");
        furniturePrototypes = new Dictionary<string, Furniture>();
        Furniture wallPrototype = Furniture.CreatePrototype(
            "Wall",
            0,
            1,
            1,
            true // links to neighbors and sort of becomes part of a larger object
        );
        furniturePrototypes.Add("Wall", wallPrototype);
    }


    public void RandomizeTiles()
    {
        Debug.Log("Randomizing Tiles");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    tiles[x, y, 0].type = TileType.Empty;
                }
                else
                {
                    tiles[x, y, 0].type = TileType.Floor;
                }
            }
        }
    }

    public Tile GetTileAt(int x, int y, int level)
    {
        if (x > _width || x < 0 || y > _height || y < 0)
        {
            Debug.LogError("Tile (" + x + ", " + y + ", " + level + ") is out of range.");
            return null;
        }
        return tiles[x, y, level];
    }

    public void PlaceFurniture(string objectType, Tile t) {
        // TODO: this function assumes 1x1 tiles -- this needs to be changed
        if (furniturePrototypes.ContainsKey(objectType) == false) {
            Debug.LogError("furniturePrototypes does nto contain prototype for key: " + objectType);
            return;
        }

        Furniture obj = Furniture.PlaceInstance( furniturePrototypes[objectType], t);

        if(obj == null) {
            // Failed to place object -- most likely there was already something there
            return;
        }

        if (cbFurnitureCreated != null) {
            cbFurnitureCreated(obj);
        }
    }

    public void RegisterFurnitureCreated(Action<Furniture> callbackFunc) {
        cbFurnitureCreated += callbackFunc;
    }
    public void UnregisterFurnitureCreated(Action<Furniture> callbackFunc) {
        cbFurnitureCreated -= callbackFunc;
    }
}
