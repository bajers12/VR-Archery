using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
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
    public WFCTileData tileSO;
    public List<Orientation> orientations;
}

[CreateAssetMenu(fileName = "newWFCTIleData", menuName = "WFCTile")]
public class WFCTileData : ScriptableObject
{

    [SerializeField]
    public GameObject tilePrefab;

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

        rotatedPermutations = new Dictionary<Orientation, WFCTile> { {Orientation.NORTH, northRotation }, { Orientation.EAST, eastRotation }, {Orientation.SOUTH, southRotation}, {Orientation.WEST, westRotation} };
        
        allConnections = new Dictionary<Direction, List<Connection>>{   {Direction.NORTH, northConnections}, { Direction.EAST, eastConnections }, { Direction.SOUTH, southConnections }, 
                                                                        { Direction.WEST, westConnections }, { Direction.UP, upConnections }, { Direction.DOWN, downConnections } };
    }

    public List<WFCTile> getAllRotatedPermutations()
    {
        return rotatedPermutations.Values.ToList();
    }

    public List<WFCTile> GetPossibleNeighbours(Orientation tileOrientation, Direction connectionDirection)
    {
        List<WFCTile> connectedTiles = new List<WFCTile>();
        if ((int)connectionDirection <= 3) 
        {
            Direction correctedConnectionDirection = (Direction) (((int)connectionDirection - (int)tileOrientation + 4) % 4);

            foreach (Connection connection in allConnections[correctedConnectionDirection])
            {
                foreach (Orientation orientation in connection.orientations)
                {
                    connectedTiles.Add(connection.tileSO.rotatedPermutations[(Orientation) (((int)orientation + (int)tileOrientation)%4)]);
                }
            }
        }
        else
        {
            foreach (Connection connection in allConnections[connectionDirection])
            {
                foreach (Orientation orientation in connection.orientations)
                {
                    connectedTiles.Add(connection.tileSO.rotatedPermutations[(Orientation)(((int)orientation + (int)tileOrientation) % 4)]);
                }
            }
        }
        return connectedTiles;
    }

}