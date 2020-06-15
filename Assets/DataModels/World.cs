using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    Tile[,,] tiles;
    int _width;
    public int width {
        get {
            return _width;
        }
    }
    int _height;
    public int height {
        get {
            return _height;
        }
    }
    int _vertical_height;
    int _vertical_depth;

    public World(int width = 150, int height = 150) {
        _vertical_height = 0;
        _vertical_depth = 0;
        this._width = width;
        this._height = height;
        tiles = new Tile[width,height,1];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                tiles[x,y,0] = new Tile(this, x, y, 0);
            }
        }
        RandomizeTiles();
        Debug.Log ("World created with " + width * height + " tiles.");
    }

    public void RandomizeTiles() {
        Debug.Log("Randomizing Tiles");
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if(Random.Range(0, 2) == 0) {
                    tiles[x,y,0].type = TileType.Empty;
                }
                else {
                    tiles[x,y,0].type = TileType.Floor;
                }
            }
        }
    }

    public Tile GetTileAt(int x, int y, int level) {
        if(x > _width || x < 0 || y > _height || y < 0) {
            Debug.LogError("Tile (" + x + ", " + y + ", " + level + ") is out of range.");
            return null;
        }
        return tiles[x,y,level];
    }
}
