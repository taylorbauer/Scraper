using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class World
{
    Tile[,,] tiles;

    Dictionary<string, InstalledObject> installedObjectPrototypes;

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

    Action<InstalledObject> cbInstalledObjectCreated;

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

        CreateInstalledObjectPrototypes();


    }

    void CreateInstalledObjectPrototypes()
    {
        installedObjectPrototypes = new Dictionary<string, InstalledObject>();
        InstalledObject wallPrototype = InstalledObject.CreatePrototype(
            "Wall",
            0,
            1,
            1
        );
        installedObjectPrototypes.Add("Wall", wallPrototype);
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

    public void PlaceInstalledObject(string objectType, Tile t) {
        // TODO: this function assumes 1x1 tiles -- this needs to be changed
        if (installedObjectPrototypes.ContainsKey(objectType) == false) {
            Debug.LogError("installedObjectPrototypes does nto contain prototype for key: " + objectType);
            return;
        }

        InstalledObject obj = InstalledObject.PlaceInstance( installedObjectPrototypes[objectType], t);

        if(obj == null) {
            // Failed to place object -- most likely there was already something there
            return;
        }

        if (cbInstalledObjectCreated != null) {
            cbInstalledObjectCreated(obj);
        }
    }

    public void RegisterInstalledObjectCreated(Action<InstalledObject> callbackFunc) {
        cbInstalledObjectCreated += callbackFunc;
    }
    public void UnregisterInstalledObjectCreated(Action<InstalledObject> callbackFunc) {
        cbInstalledObjectCreated -= callbackFunc;
    }
}
