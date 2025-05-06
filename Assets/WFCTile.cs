using System.Collections.Generic;
using UnityEngine;

public class WFCTile
{
    Orientation orientation { get; }
    WFCTileData data { get; }

    public WFCTile(WFCTileData data, Orientation rotation)
    {
        this.data = data;
        this.orientation = rotation;

    }



    public List<WFCTile> GetConnectedTiles(Direction direction)
    {
        return data.GetConnectedTiles(orientation, direction);
    }

    public override string ToString()
    {
        return data.name + orientation;
    }
}