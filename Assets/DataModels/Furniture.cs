using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Furnitures are things that can't be picked up and moved, like walls, furniture, doors, etc

public class Furniture 
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

    public bool linksToNeighbor { get; protected set;}

    Action<Furniture> cbOnChanged;

    // TODO: Implement larger objects
    // TODO: Implement object rotation

    // This is protected because we don't want other classes to be able to
    // create empty objects
    protected Furniture() { 
    }

    // This constructor is used by our "object factory" to create the prototypical object
    static public Furniture CreatePrototype(string objectType, float movementCost = 1f, int width = 1, int height = 1, bool linksToNeighbor = false) {
        Furniture obj = new Furniture();
        obj.objectType = objectType;
        obj.movementCost = movementCost;
        obj.width = width;
        obj.height = height;
        obj.linksToNeighbor = linksToNeighbor;

        return obj;
    }

    static public Furniture PlaceInstance(Furniture proto, Tile tile) {
        Furniture copy = new Furniture();
        copy.objectType = proto.objectType;
        copy.movementCost = proto.movementCost;
        copy.width = proto.width;
        copy.height = proto.height;
        copy.linksToNeighbor = proto.linksToNeighbor;

        copy.tile = tile;
        // FIXME: This is assuming every object is 1x1
        if(tile.PlaceObject(copy) == false) {
            // For some reason, we couldn't place the object in the tile
            // It is likely already occupied
            Debug.Log("Unable to place Furniture");
            return null;
        }

        if (copy.linksToNeighbor)
        { // If it links to its neighbors, gotta update them also
            int x = tile.x;
            int y = tile.y;

            Tile t = tile.world.GetTileAt(x, y + 1, 0);
            if (t != null && t.furniture != null && t.furniture.objectType == copy.objectType)
            {
                t.furniture.cbOnChanged(t.furniture);
            }
            t = tile.world.GetTileAt(x + 1, y, 0);
            if (t != null && t.furniture != null && t.furniture.objectType == copy.objectType)
            {
                t.furniture.cbOnChanged(t.furniture);
            }
            t = tile.world.GetTileAt(x, y - 1, 0);
            if (t != null && t.furniture != null && t.furniture.objectType == copy.objectType)
            {
                t.furniture.cbOnChanged(t.furniture);
            }
            t = tile.world.GetTileAt(x - 1, y, 0);
            if (t != null && t.furniture != null && t.furniture.objectType == copy.objectType)
            {
                t.furniture.cbOnChanged(t.furniture);
            }
        }

        return copy;
    }

    public void RegisterOnChangedCallback(Action<Furniture> callbackFunc) {
        cbOnChanged += callbackFunc;
    }
    public void UnregisterOnChangedCallback(Action<Furniture> callbackFunc) {
        cbOnChanged -= callbackFunc;
    }

}
