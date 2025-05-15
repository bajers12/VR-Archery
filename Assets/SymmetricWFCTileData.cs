using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "standardWFCTile", menuName = "WFCTile/Symmetric")]
public class SymmetricWFCTileData : WFCTileDataBase
{
    WFCTile tile;

    //All horizontal sides are equal, some are just more equal than other
    //NMorth conenctiosn counts for all horizontal, just added north to specify that we describe the connection in north as base orientation
    [SerializeField]
    List<Connection> northConnections;
    [SerializeField]
    List<Connection> upConnections;
    [SerializeField]
    List<Connection> downConnections;

    private void OnEnable()
    {
        tile = new WFCTile(this, Orientation.NORTH);
    }

    //Only to be used when initially creating tile map
    public override List<WFCTile> GetAllRotatedPermutations()
    {
        return new List<WFCTile>() { tile };
    }


    public override List<WFCTile> GetPossibleNeighbours(Orientation tileOrientation, Direction connectionDirection)
    {
        List<Connection> chosenConnections;
        List<WFCTile> tiles;
        Orientation orientationOffset;
        if(connectionDirection == Direction.UP || connectionDirection == Direction.DOWN)
        {
            orientationOffset = 0;
            chosenConnections = connectionDirection == Direction.UP ? upConnections : downConnections;
        } else
        {
            orientationOffset = (Orientation) connectionDirection;
            chosenConnections = northConnections;
        }
        tiles = chosenConnections.SelectMany(con => con.orientations.Select(orientation => con.tileSO.GetOrientedTile(GetFinalOrientation(orientation, orientationOffset)))).ToList();
        return tiles;
    }


    public override WFCTile GetOrientedTile(Orientation orientation)
    {
        return tile;
    }

    private Orientation GetFinalOrientation(Orientation connectionOrientation, Orientation orientationOffset)
    {
        return (Orientation)(((int)connectionOrientation + (int)orientationOffset) % 4);
    }
}
