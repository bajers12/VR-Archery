using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


internal class WFCGenerator
{
    static Vector3Int[] NESWUD = { Vector3Int.forward, Vector3Int.right, Vector3Int.back, Vector3Int.left, Vector3Int.up, Vector3Int.down };
    List<WFCTile> tileSet;
    List<WFCTile>[,,] waveGrid;

    //Provide tileset without rotation permutations
    public WFCGenerator(TileSet tileSet)
    {
        this.tileSet = tileSet.allRotatedPermutations();
    }

    public List<WFCTile>[,,] Generate(Vector3Int dimensions)
    {
        waveGrid = new List<WFCTile>[dimensions.x, dimensions.y, dimensions.z];
        FillWaveGrid(waveGrid);
        while(WFC())
        {
            continue;
        }


        return waveGrid;
    }
    void FillWaveGrid(List<WFCTile>[,,] waveGrid)
    {
        for (int i = 0; i < waveGrid.GetLength(0); i++)
        {
            for (int j = 0; j < waveGrid.GetLength(1); j++)
            {
                for (int k = 0; k < waveGrid.GetLength(2); k++)
                {
                    waveGrid[i, j, k] = new List<WFCTile>(tileSet);
                }
            }
        }
    }
    bool WFC()
    {
        try
        {
            Vector3Int observedCell = Observe();
            Propagate(observedCell);
            return true;
        } catch
        {
            return false;
        }
    }



    //Finds cell with lowest evaluation function, determines a specific tile, and returns index of observed cell.
    Vector3Int Observe()
    {
        Vector3Int chosenCell = FindRandomBestCandidate();
        WFCTile chosenTile = PickTile(chosenCell);
        CollapseCell(chosenCell, chosenTile);
        return chosenCell;
    }

    void CollapseCell(Vector3Int cell, WFCTile tile)
    {
        waveGrid[cell.x, cell.y, cell.z] = new List<WFCTile>() { tile };

    }

    Vector3Int FindRandomBestCandidate()
    {
        List<Vector3Int> candidates = new List<Vector3Int>();
        float minEntropyObserved = float.MaxValue;
        Vector3Int cell = new Vector3Int();
        float entropy;
        for (int i = 0; i < waveGrid.GetLength(0); i++)
        {
            for (int j = 0; j < waveGrid.GetLength(1); j++)
            {
                for (int k = 0; k < waveGrid.GetLength(2); k++)
                {
                    cell.Set(i, j, k);
                    if(IsCollapsed(cell))
                    {
                        continue;
                    }
                    entropy = Entropy(cell);
                    if(entropy < minEntropyObserved)
                    {
                        minEntropyObserved = entropy;
                        candidates = new List<Vector3Int>() { cell };
                    } else if(entropy == minEntropyObserved)
                    {
                        candidates.Add(cell);
                    }
                }
            }
        }
        //TODO return random cell 
        return candidates[Random.Range(0, candidates.Count)];
    }

    private bool IsCollapsed(Vector3Int cell)
    {
        return waveGrid[cell.x, cell.y, cell.z].Count == 1;
    }

    float Entropy(Vector3Int cell)
    {
        float entropy = 0;
        foreach(WFCTile tile in waveGrid[cell.x, cell.y, cell.z])
        {
            entropy += 1;
        }
        return entropy;
    }

    //Collapses wave function for a specific cell and returns 
    WFCTile PickTile(Vector3Int cell)
    {
        List<WFCTile> tileSuperpositions = waveGrid[cell.x, cell.y, cell.z];
        return tileSuperpositions[Random.Range(0, tileSuperpositions.Count)];
    }

    void Propagate(Vector3Int cell)
    {
        Vector3Int[] neighbours = CellNeighbours(cell);
        Vector3Int neighbour;
        foreach(Direction dir in Enum.GetValues(typeof(Direction)))
        {
            neighbour = neighbours[(int)dir];
            if (neighbour != null) {
                removeUnviableTiles(neighbour, cell, dir);
            }
        }
    }

    Vector3Int[] CellNeighbours(Vector3Int cell)
    {
        Vector3Int[] neighbours = new Vector3Int[6];
        Vector3Int neighbour;
        for(int i =0; i<neighbours.Length; i++)
        {
            neighbour = cell + NESWUD[i];
            if(InGrid(neighbour))
            {
                neighbours[i] = neighbour;
            }
        }
        return neighbours;
    }

    bool InGrid(Vector3Int cell)
    {
        return (cell.x < waveGrid.GetLength(0) && cell.x >= 0  ) && (cell.y < waveGrid.GetLength(1) && cell.y >= 0) && (cell.z < waveGrid.GetLength(2) && cell.z >= 0);
    }

    //TODO remove all tiles from neighbours which do not fit this
    void removeUnviableTiles(Vector3Int neighbour, Vector3Int collapsedCell, Direction connectionDirection)
    {
        List<WFCTile> previousPossibleTiles = waveGrid[neighbour.x, neighbour.y, neighbour.z];
        WFCTile collapsedTile = waveGrid[collapsedCell.x, collapsedCell.y, collapsedCell.z][0];
        List<WFCTile> collapsedTileConnections = collapsedTile.GetConnectedTiles(connectionDirection);
        List<WFCTile> possibleTiles = previousPossibleTiles.Where(tile => collapsedTileConnections.Contains(tile)).ToList(); 
    }

}