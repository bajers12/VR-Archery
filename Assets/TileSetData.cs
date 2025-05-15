using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newTileSet", menuName = "TileSet")]
internal class TileSetData : ScriptableObject
{
    public WFCTileDataBase ground;
    public WFCTileDataBase air;
    public List<WFCTileDataBase> tiles;

    public List<WFCTile> allRotatedPermutations()
    {
        List<WFCTile> rotatedPermutations = new List<WFCTile>();
        foreach(WFCTileDataBase tileData in tiles)
        {
            foreach(WFCTile rotatedTile in tileData.GetAllRotatedPermutations())
            {
                rotatedPermutations.Add(rotatedTile);
            }
        }
        return rotatedPermutations;
    }
}