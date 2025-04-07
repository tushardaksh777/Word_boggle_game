using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class DataModel
{
    public int bugCount;
    public int wordCount;
    public int timeSec;
    public int totalScore;
    public GridSize gridSize;
    public List<GridData> gridData;
}
[Serializable]
public class GridSize
{
    public GridSize(int _x , int _y)
    {
        x = _x;
        y = _y;
    }
    public int x;
    public int y;
}
[Serializable]
public class GridData
{
    public int tileType;
    public string letter;
}

public class Data
{
    public List<DataModel> data;
}
