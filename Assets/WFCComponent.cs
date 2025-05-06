using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WFCComponent : MonoBehaviour
{
    private WFCGenerator waveFunctionGenerator;

    [SerializeReference]
    private TileSet tileSet;
    [SerializeField]
    private Vector3Int dimensions = new Vector3Int(3,3,3);

    public List<WFCTileData>[,,] generatedTiles;

    private void Start()
    {
        waveFunctionGenerator = new WFCGenerator(tileSet);
        List<WFCTile>[,,] generatedTiles = waveFunctionGenerator.Generate(dimensions);
        for (int i = 0; i < generatedTiles.GetLength(0); i++)
        {
            for (int j = 0; j < generatedTiles.GetLength(1); j++)
            {
                for (int k = 0; k < generatedTiles.GetLength(2); k++)
                {
                    Debug.Log(generatedTiles[i, j, k][0]);
                }
            }
        }

    }



}
