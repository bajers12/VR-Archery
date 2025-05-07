using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newTileSet", menuName = "TileSet")]
internal class TileSet : ScriptableObject
{
    public List<WFCTileData> tiles;

    public List<WFCTile> allRotatedPermutations()
    {
        List<WFCTile> rotatedPermutations = new List<WFCTile>();
        foreach(WFCTileData tileData in tiles)
        {
            foreach(WFCTile rotatedTile in tileData.getAllRotatedPermutations())
            {
                rotatedPermutations.Add(rotatedTile);
            }
        }
        return rotatedPermutations;
    }
}