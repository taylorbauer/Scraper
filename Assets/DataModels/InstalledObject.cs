using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// InstalledObjects are things that can't be picked up and moved, like walls, furniture, doors, etc

public class InstalledObject 
{
    // This rerpresents the *base* tile of an object, but in practice, large objects can
    // occupy multiple tiles
    Tile tile; // It should know which tile it's in
    // Don't need to know x and y coordinate, because the tile knows that
   
    string objectType; // This will pretty much map to the graphic

    // A movement cost is a multiplier, so a value of 2 means you move twice as slowly (i.e. at half speed)
    // Tile types and other envronmental effects may further act as multipliers, like a rough tile (cost 2) 
    // with a table (cost 3) that is on fire (cost 3) would have a total movement cost of (2 + 3 + 3), so 
    // you'd move through this tile at 1/8 normal speed
    // SPECIAL: if movement cost is =0, then this tile is impassable
    float movementCost = 1f; 
    
    int width = 1;
    int height = 1;
}
