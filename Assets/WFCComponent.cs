using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCComponent : MonoBehaviour
{
    private WFCGenerator waveFunctionGenerator;

    [SerializeReference]
    private TileSetData tileSet;
    [SerializeField]
    private Vector3Int gridDimensions = new Vector3Int(3,3,3);
    [SerializeField]
    public float tileScale = 1f;

    public List<StandardWFCTileData>[,,] generatedTiles;

    private void Start()
    {
        waveFunctionGenerator = new WFCGenerator(tileSet);
        List<WFCTile>[,,] generatedTiles = waveFunctionGenerator.Generate(gridDimensions);
        for (int i = 0; i < generatedTiles.GetLength(0); i++)
        {
            for (int j = 0; j < generatedTiles.GetLength(1); j++)
            {
                for (int k = 0; k < generatedTiles.GetLength(2); k++)
                {
                    List<WFCTile> tileSuperpositions = generatedTiles[i, j, k];
                    if(tileSuperpositions.Count == 1)
                    {
                        GameObject tile = Instantiate(generatedTiles[i, j, k][0].GetPrefab(), new Vector3(i, j, k) * tileScale, generatedTiles[i, j, k][0].GetPrefab().transform.rotation);
                        tile.name = "" + i + ", " + j + ", " + k;
                    }
                }
            }
        }

    }



}
