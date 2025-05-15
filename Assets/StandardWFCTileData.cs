using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]


[CreateAssetMenu(fileName = "standardWFCTile", menuName = "WFCTile/Standard")]
public class StandardWFCTileData : WFCTileDataBase
{
    [SerializeField]
    List<Connection> northConnections;
    [SerializeField]
    List<Connection> eastConnections;
    [SerializeField]
    List<Connection> southConnections;
    [SerializeField]
    List<Connection> westConnections;
    [SerializeField]
    List<Connection> upConnections;
    [SerializeField]
    List<Connection> downConnections;

    Dictionary<Direction, List<Connection>> allConnections;
    Dictionary<Orientation, WFCTile> rotatedPermutations;

    void OnEnable()
    {
        WFCTile northRotation = new WFCTile(this, Orientation.NORTH);
        WFCTile eastRotation = new WFCTile(this, Orientation.EAST);
        WFCTile southRotation = new WFCTile(this, Orientation.SOUTH);
        WFCTile westRotation = new WFCTile(this, Orientation.WEST);

        rotatedPermutations = new Dictionary<Orientation, WFCTile> { { Orientation.NORTH, northRotation }, { Orientation.EAST, eastRotation }, { Orientation.SOUTH, southRotation }, { Orientation.WEST, westRotation } };

        allConnections = new Dictionary<Direction, List<Connection>>{   {Direction.NORTH, northConnections}, { Direction.EAST, eastConnections }, { Direction.SOUTH, southConnections },
                                                                        { Direction.WEST, westConnections }, { Direction.UP, upConnections }, { Direction.DOWN, downConnections } };
    }

    public override List<WFCTile> GetAllRotatedPermutations()
    {
        return rotatedPermutations.Values.ToList();
    }

    public override List<WFCTile> GetPossibleNeighbours(Orientation tileOrientation, Direction connectionDirection)
    {
        List<WFCTile> connectedTiles = new List<WFCTile>();
        Orientation connectionCorrectedOrientation;
        if ((int)connectionDirection <= 3)
        {
            Direction correctedConnectionDirection = (Direction)(((int)connectionDirection - (int)tileOrientation + 4) % 4);

            foreach (Connection connection in allConnections[correctedConnectionDirection])
            {
                foreach (Orientation connectionOrientation in connection.orientations)
                {
                    connectionCorrectedOrientation = GetFinalOrientation(connectionOrientation, tileOrientation);
                    connectedTiles.Add(connection.tileSO.GetOrientedTile(connectionCorrectedOrientation));
                }
            }
        }
        else
        {
            foreach (Connection connection in allConnections[connectionDirection])
            {
                foreach (Orientation connectionOrientation in connection.orientations)
                {
                    connectionCorrectedOrientation = GetFinalOrientation(connectionOrientation, tileOrientation);
                    connectedTiles.Add(connection.tileSO.GetOrientedTile(connectionCorrectedOrientation));
                }
            }
        }
        return connectedTiles;
    }

    public override WFCTile GetOrientedTile(Orientation orientation)
    {
        return rotatedPermutations[orientation];
    }

    protected Orientation GetFinalOrientation(Orientation connectionOrientation, Orientation tileOrientation)
    {
        return (Orientation)(((int)connectionOrientation + (int)tileOrientation) % 4);
    }
}