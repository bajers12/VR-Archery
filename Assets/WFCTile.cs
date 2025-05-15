using System.Collections.Generic;
using UnityEngine;

public class WFCTile
{
    Orientation orientation;
    public IWFCTileData data { get; }

    public WFCTile(IWFCTileData data, Orientation rotation)
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
        return "" + data + orientation;
    }

    public float GetRotation()
    {
        return 90 * (int)orientation;
    }

    public GameObject GetPrefab()
    {
        return data.GetPrefab();
    }
}