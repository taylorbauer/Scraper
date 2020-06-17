using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// InstalledObjects are things that can't be picked up and moved, like walls, furniture, doors, etc

public class InstalledObject 
{
    // This rerpresents the *base* tile of an object, but in practice, large objects can
    // occupy multiple tiles
    public Tile tile {get; protected set;} // It should know which tile it's in
    // Don't need to know x and y coordinate, because the tile knows that
   
    public string objectType {get; protected set;} // This will pretty much map to the graphic

    // A movement cost is a multiplier, so a value of 2 means you move twice as slowly (i.e. at half speed)
    // Tile types and other envronmental effects may further act as multipliers, like a rough tile (cost 2) 
    // with a table (cost 3) that is on fire (cost 3) would have a total movement cost of (2 + 3 + 3), so 
    // you'd move through this tile at 1/8 normal speed
    // SPECIAL: if movement cost is =0, then this tile is impassable
    float movementCost; 
    
    int width;
    int height;

    Action<InstalledObject> cbOnChanged;

    // TODO: Implement larger objects
    // TODO: Implement object rotation

    // This is protected because we don't want other classes to be able to
    // create empty objects
    protected InstalledObject() { 
    }

    // This constructor is used by our "object factory" to create the prototypical object
    static public InstalledObject CreatePrototype(string objectType, float movementCost = 1f, int width = 1, int height = 1) {
        InstalledObject obj = new InstalledObject();
        obj.objectType = objectType;
        obj.movementCost = movementCost;
        obj.width = width;
        obj.height = height;

        return obj;
    }

    static public InstalledObject PlaceInstance(InstalledObject proto, Tile tile) {
        InstalledObject copy = new InstalledObject();
        copy.objectType = proto.objectType;
        copy.movementCost = proto.movementCost;
        copy.width = proto.width;
        copy.height = proto.height;

        copy.tile = tile;
        // FIXME: This is assuming every object is 1x1
        if(tile.PlaceObject(copy) == false) {
            // For some reason, we couldn't place the object in the tile
            // It is likely already occupied
            Debug.Log("Unable to place InstalledObject");
            return null;
        }

        return copy;
    }

    public void RegisterOnChangedCallback(Action<InstalledObject> callbackFunc) {
        cbOnChanged += callbackFunc;
    }
    public void UnregisterOnChangedCallback(Action<InstalledObject> callbackFunc) {
        cbOnChanged -= callbackFunc;
    }

}
