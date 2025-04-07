using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPatcher : MonoBehaviour
{
    [SerializeField]
    TextAsset LevelsDataresponse;

    [SerializeField]
    TextAsset EndlessDataresponse;

    public DataModel GetDataFromTextFile_Endless()
    {
        Data model = JsonUtility.FromJson<Data>(EndlessDataresponse.text);
        return model.data[1];
    }

    public DataModel GetDataFromTextFile()
    {
        Data model = JsonUtility.FromJson<Data>(LevelsDataresponse.text);
        Debug.Log(model.data.Count);
        return model.data[0];
    }
}

/*
{
  "bugCount": 0,
  "wordCount": 4,
  "timeSec": 0,
  "totalScore": 0,
  "gridSize": { "x": 4, "y": 4 },
  "gridData": [
    { "tileType": 0, "letter": "A" },
    { "tileType": 0, "letter": "B" }
  ]
};

*/
