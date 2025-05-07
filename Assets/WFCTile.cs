using System.Collections.Generic;
using UnityEngine;

public class WFCTile
{
    public Orientation orientation { get; }
    WFCTileData data { get; }

    public WFCTile(WFCTileData data, Orientation rotation)
    {
        this.data = data;
        this.orientation = rotation;

    }



    public List<WFCTile> GetPossibleNeighbours(Direction direction)
    {
        return data.GetPossibleNeighbours(orientation, direction);
    }

    public override string ToString()
    {
        return data.name + orientation;
    }

    public GameObject getPrefab()
    {
        return data.tilePrefab;
    }
}