using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoCandidatesException : Exception
{
    public NoCandidatesException() : base() { }

    public NoCandidatesException(string message) : base(message) { }
}

public class NoOptionsException : Exception
{
    public NoOptionsException() : base() { }

    public NoOptionsException(string message) : base(message) { }
}

internal class WFCGenerator
{

    public static Vector3Int[] NESWUD = { Vector3Int.forward, Vector3Int.right, Vector3Int.back, Vector3Int.left, Vector3Int.up, Vector3Int.down };
    List<WFCTile> tileSet;
    List<WFCTile>[,,] waveGrid;

    //Provide tileset without rotation permutations
    public WFCGenerator(TileSet tileSet)
    {
        this.tileSet = tileSet.allRotatedPermutations();
        Debug.Log(this + " has " + this.tileSet.Count + " tile permutations");
    }

    public List<WFCTile>[,,] Generate(Vector3Int dimensions)
    {
        waveGrid = new List<WFCTile>[dimensions.x, dimensions.y, dimensions.z];
        FillWaveGrid(waveGrid);

        int populated_tilecount = 0;
        while(WFC())
        {
            populated_tilecount++;
            continue;
        }

        Debug.Log("Tiles generated " + populated_tilecount);
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
            return true;
        } catch (Exception e)
        {
            Debug.Log(e);
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
        Propagate(cell);
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
        if(candidates.Count == 0)
        {
            throw new NoCandidatesException("no more valid candidates");
        }
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
        if(tileSuperpositions.Count == 0)
        {
            throw new NoOptionsException("Novalid options at " + cell);
        }
        return tileSuperpositions[Random.Range(0, tileSuperpositions.Count)];
    }

    void Propagate(Vector3Int collapsedCell)
    {
        Vector3Int[] neighbours = CellNeighbours(collapsedCell);
        Vector3Int neighbour;

        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            neighbour = neighbours[(int)dir];
            if(!InGrid(neighbour) || IsCollapsed(neighbour))
            {
                continue;
            }

            int removedOptions = removeUnviableTiles(neighbour, collapsedCell, dir);
            if(removedOptions > 0)
            {
                Propagate(neighbour);
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
            neighbours[i] = neighbour;
        }
        return neighbours;
    }

    bool InGrid(Vector3Int cell)
    {
        bool inGrid = (cell.x < waveGrid.GetLength(0) && cell.x >= 0) && (cell.y < waveGrid.GetLength(1) && cell.y >= 0) && (cell.z < waveGrid.GetLength(2) && cell.z >= 0);
        return inGrid;
    }

    int removeUnviableTiles(Vector3Int targetCellIndex, Vector3Int propagatedCellIndex, Direction connectionDirection)
    {
        List<WFCTile> targetCell = waveGrid[targetCellIndex.x, targetCellIndex.y, targetCellIndex.z];
        List<WFCTile> propagatedCell = waveGrid[propagatedCellIndex.x, propagatedCellIndex.y, propagatedCellIndex.z];

        Debug.Log("Removing options in direction " + connectionDirection + " from " + propagatedCellIndex + propagatedCell[0]);

        int optionsPreRemoval = targetCell.Count;
        List<WFCTile> propagatedCellConnections = propagatedCell.SelectMany(tile => tile.GetPossibleNeighbours(connectionDirection)).ToList();
        targetCell.RemoveAll(tile => !propagatedCellConnections.Contains(tile));
        if(targetCell.Count == 0)
        {
            new InvalidOperationException(targetCellIndex + " has no possible tiles after propagation from " + propagatedCell);
        }
        return optionsPreRemoval - targetCell.Count;
    }

}