using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public List<ColumnManager> columnManagers;

    public event StartRemovingBlocks startRemovingBlock;
    public delegate void StartRemovingBlocks();

    public Action updateIds;

    int gridX = 4;
    int gridY = 4;
    private void Awake()
    {
        SetIds();
        //GameManager.Instance.onDataRecevied += GameManager_OnDataReceived;
    }
    private void Start()
    {
        GameManager.Instance.onWordValidate += GameManager_onWordValidate;
        GameManager.Instance.CheckForBonusBlocks += IsContainsBonus;
        updateIds += SetIds;
    }
    private void SetIds()
    {
        for (int i = 0; i < columnManagers.Count; i++)
        {
            ColumnManager columnManager = columnManagers[i];
            columnManager.SetIds(i, gridY);
        }
    }

    public bool IsAdjacent(int Adjacentid , int lastId)
    {
        List<BlockHandler> adjacent =  GetAdjacent(lastId);

        foreach (var item in adjacent)
        {
            if(item.id == Adjacentid)
            {
                return true;
            }
        }
        return false;
    }

    //Getting adjacent of given Id
    public List<BlockHandler> GetAdjacent(int lastId)
    {
        int xAxis = lastId / columnManagers.Count;  // Row Index
        int yAxis = lastId % columnManagers.Count;  // Column Index

        List<GridSize> directions = GetDirection();
        List<BlockHandler> adjacent = new List<BlockHandler>();   //Holding Adjacent Ids
        foreach (var item in directions)
        {
            int x = xAxis + item.x;
            int y = yAxis + item.y;
            if ((x >= 0 && x < columnManagers.Count) && (y >= 0 && y < columnManagers.Count))
            {
                BlockHandler block = GetBlockByRowAndColumn(x, y);
                adjacent.Add(block);
                Debug.Log("Adjacent of " + lastId + " is " + block.id);
            }
        }
        return adjacent;
    }

    //Adjacent Direction
    private List<GridSize> GetDirection()
    {
        List<GridSize> gridSizes = new List<GridSize>()
        {
            new GridSize(0,1), // right
            new GridSize(0,-1),// left
            new GridSize(-1,0),// top
            new GridSize(1,0),// bottom
            new GridSize(-1,-1),// top left
            new GridSize(-1,1),// top right
            new GridSize(1,-1),// bottom left
            new GridSize(1,1),// bottom right
        };

        return gridSizes;
    }

    private BlockHandler GetBlockByRowAndColumn(int row , int column)
    {
        return columnManagers[column].blockHandlers[row];
    }

    private void UnlockBlocks(List<IBlockBehaviour> selectedTiles) //Finding locked block and unlocking them
    {
        List<IBlockBehaviour> LockedBlocks = new List<IBlockBehaviour>();
        foreach (var item in selectedTiles)
        {
            List<BlockHandler> adjacent = GetAdjacent(item.blockId);
            foreach (var item1 in adjacent)
            {
                IBlockBehaviour block = item1.GetComponent<IBlockBehaviour>();
                if (block.blockType == BlockType.Locked)
                {
                    block.OnUpdateType(BlockType.Normal);
                }
            }
        }
    }

    private void GameManager_onWordValidate() // When Word is matched with the dictionary
    {
        UnlockBlocks(GameManager.Instance.selectedBlocks);
        if(GameManager.Instance.gameMode == GameMode.Endless)
        {
            GameManager.Instance.blockInputs.Invoke(true);
            RemoveSelectedBlocks();
        }  
    }

    public bool IsContainsBonus()
    {
        if(FindBonus().Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }  
    }
    private List<IBlockBehaviour> FindBonus()
    {
        List<IBlockBehaviour> blocks = GameManager.Instance.selectedBlocks; 
        List<IBlockBehaviour> BonusBlocks = new List<IBlockBehaviour>();
        foreach (var block in blocks)
        {
            if(block.blockType == BlockType.Bonus)
            {
                BonusBlocks.Add(block);
            }
        }
        return BonusBlocks;
    }

    private void RemoveSelectedBlocks() 
    {
        //Removing Selected Blocks 
        //Seprate blocks as per their column and performing Shifting of Blocks in ColumnManager

        List<IBlockBehaviour> blocks = GameManager.Instance.selectedBlocks;
        foreach (var item in blocks)
        {
            int xAxis = item.blockId / columnManagers.Count;  // Row Index
            int yAxis = item.blockId % columnManagers.Count;  // Column Index
            columnManagers[yAxis].AddRemovableBlocks(xAxis);
        }
        startRemovingBlock.Invoke();
    }
}
