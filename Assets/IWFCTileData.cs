using System.Collections.Generic;
using UnityEngine;

public enum Orientation
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}

public enum Direction
{
    NORTH,
    EAST,
    SOUTH,
    WEST,
    UP,
    DOWN
}

[System.Serializable]
public struct Connection
{
    public WFCTileDataBase tileSO;
    public List<Orientation> orientations;
}
public interface IWFCTileData
{
    GameObject GetPrefab();

    List<WFCTile> GetAllRotatedPermutations();
    List<WFCTile> GetPossibleNeighbours(Orientation tileOrientation, Direction connectionDirection);

    WFCTile GetOrientedTile(Orientation orientation);

}