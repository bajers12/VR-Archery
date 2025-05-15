using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public abstract class WFCTileDataBase : ScriptableObject, IWFCTileData
{
    [SerializeReference]
    private GameObject tilePrefab;
    public abstract List<WFCTile> GetAllRotatedPermutations();
    public abstract WFCTile GetOrientedTile(Orientation orientation);
    public abstract List<WFCTile> GetPossibleNeighbours(Orientation tileOrientation, Direction connectionDirection);


    public GameObject GetPrefab()
    {
        return tilePrefab;
    }
}