using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TileType {Empty, Floor};
public class Tile 
{
    Action<Tile> tileTypeChangedCallback;

    TileType _type = TileType.Floor;
    public TileType type {
        get {
            return _type;
        }
        set {
            _type = value;
            // Delegates are variables in C# that hold a reference to a function
            // An action is a shortcut for a delegate
            if (tileTypeChangedCallback != null) {
                tileTypeChangedCallback(this);
            }
        }
    }

    Inventory looseObject;
    public Furniture furniture {get; protected set;}

    public World world {get; protected set;}
    int _x;
    public int x {
        get {
            return _x;
        }
    }
    int _y;
    public int y {
        get {
            return _y;
        }
    }
    int level;

    public Tile(World world, int x, int y, int level){
        this.world = world;
        this._x = x;
        this._y = y;
        this.level = level;
    }

    public void registerTileTypeChangedCallback(Action<Tile> callback) {
        tileTypeChangedCallback += callback;
    }
    public void unregisterTileTypeChangedCallback(Action<Tile> callback) {
        tileTypeChangedCallback -= callback;
    }

    public bool PlaceObject(Furniture objectInstance) {
        if(objectInstance == null) {
            // we are uninstalling whatever was here before
            furniture = null;
            return true;
        }

        // objectInstance isn't null
        if (furniture != null) {
            Debug.LogError("Trying to assign an installed object to a tile that already has one");
            return false;
        }
        else {
            furniture = objectInstance;
            return true;
        }
    }
}
