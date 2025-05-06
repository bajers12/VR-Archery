using System.Collections.Generic;
using System.Linq;
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
    public Orientation orientation;
}

[CreateAssetMenu(fileName = "newWFCTIleData", menuName = "WFCTile")]
public class WFCTileData : ScriptableObject
{

    [SerializeField]
    public float baseProbabílity { get; } = 1f ;
    [SerializeField]
    GameObject tilePrefab;


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



    WFCTile northRotation;
    WFCTile eastRotation;
    WFCTile southRotation;
    WFCTile westRotation;

    List<Connection>[] allConnections;
    public WFCTile[] rotatedPermutations { get; private set; }

    void OnEnable()
    {
        Debug.Log(this + " has awoken");
        CreateRotatedPermutations();
        rotatedPermutations = new WFCTile[] { northRotation, eastRotation, southRotation, westRotation };
        allConnections = new List<Connection>[] { northConnections, eastConnections, southConnections, westConnections, upConnections, downConnections };
    }

    void CreateRotatedPermutations()
    {
        northRotation = NorthRotation();
        eastRotation = EastRotation();
        southRotation = SouthRotation();
        westRotation = WestRotation();
    }

    public List<Connection>[] Connections()
    {
        return new List<Connection>[] { northConnections, eastConnections, southConnections, westConnections, upConnections, downConnections };
    }

    public List<WFCTile> GetConnectedTiles(Orientation tileOrientation, Direction connectionDirection)
    {
        //Sideways connections directions are offset by tile orientation
        if((int)connectionDirection <= 3)
        {
            int connectedTileIndex = ((int)tileOrientation + (int)connectionDirection)%4;
            return allConnections[connectedTileIndex].Select(con => con.tileSO.rotatedPermutations[connectedTileIndex]).ToList();
        } else//Up and down connections always use same connection, but different orientations of it deppending on tile orientation
        {
            return allConnections[(int)connectionDirection].Select(con => con.tileSO.rotatedPermutations[(int)tileOrientation]).ToList();
        }
    }

    WFCTile NorthRotation()
    {
        return new WFCTile(this, Orientation.NORTH);
    }
    WFCTile EastRotation()
    {
        return new WFCTile(this, Orientation.EAST);
    }
    WFCTile SouthRotation()
    {
        return new WFCTile(this, Orientation.SOUTH);
    }
    WFCTile WestRotation()
    {
        return new WFCTile(this, Orientation.WEST);
    }


}