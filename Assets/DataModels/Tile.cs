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

    LooseObject looseObject;
    InstalledObject installedObject;

    World world;
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


}
