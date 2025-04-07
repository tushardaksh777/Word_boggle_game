using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public enum GameMode
{
    Endless,
    Levels
}
public enum GameStage
{
    SelectGameModes,
    Play

}
public enum BlockType
{
    Normal,
    Bonus,
    Locked
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GridManager gridManager;

    public List<IBlockBehaviour> selectedBlocks = new List<IBlockBehaviour>();
    public DataPatcher dataPatcher;
    public UIVisualsController visualsController;
    

    public DataModel responseData;
    public DataModel responseDataForLevels;

    public event OnDataReceived onDataRecevied;
    public delegate void OnDataReceived(DataModel data);

    public GameMode gameMode;
    public GameStage gameStage;

    public Action<IBlockBehaviour> onBlockSelect;
    public Action onBlockDeselect;

    public Action OnUpdateWord;
    public string Word;
    private List<string> submittedWord = new List<string>();

    public Action OnUpdateScore;
    public int score;

    public Func<string, bool> CheckForWordValidate; // string - word and bool is for isWord validate or not
    public Func<bool> CheckForBonusBlocks;

    public event OnWordValidate onWordValidate;
    public delegate void OnWordValidate();

    public Action<bool> blockInputs;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        
    }
    void Start()
    {
        gameMode = GameMode.Levels;
       //onDataRecevied.Invoke(responseData);
        UpdateGameStages(GameStage.SelectGameModes);
        onBlockSelect += OnSelectBlocks;
        onBlockDeselect += SumbitWord;
    }
    public void OnSelectBlocks(IBlockBehaviour block)
    {
        selectedBlocks.Add(block);
        block.OnSelect();
        Word += block.letter;
        OnUpdateWord.Invoke();
    }
    public void SumbitWord()
    {
        bool isValidate = CheckForWordValidate.Invoke(Word);
        Debug.Log("Is validate word " + isValidate + " Word " + Word);
        if (isValidate && !submittedWord.Contains(Word))
        {
            submittedWord.Add(Word);
            score += 1;
            if (CheckForBonusBlocks.Invoke())
            {
                score += 5;
            }
            OnUpdateScore.Invoke();
            onWordValidate.Invoke();
        }
        ResetSelectedBlocks();
        
    }
    public void ResetSelectedBlocks()
    {
        for (int i = 0; i < selectedBlocks.Count; i++)
        {
            selectedBlocks[i].OnDeselect();

        }
        selectedBlocks.Clear();
        Word = "";
        OnUpdateWord.Invoke();
    }
    public void UpdateGameStages(GameStage gs)
    {
        switch (gs)
        {
            case GameStage.SelectGameModes:
                gameStage = GameStage.SelectGameModes;
                score = 0;
                Word = "";
                submittedWord.Clear();
                OnUpdateScore.Invoke();
                OnUpdateWord.Invoke();
                visualsController.showSelection.Invoke();
                visualsController.HidePlayUI();
                break;
            case GameStage.Play:
                gameStage = GameStage.Play;
                visualsController.HideSelection.Invoke();
                onDataRecevied.Invoke(responseData);
                visualsController.ShowPlayUI();
                break;
        }
    }
    public string GetLetter(int id)
    {
        return responseData.gridData[id].letter;
    }

    public bool IsAdjacent(int id)
    {
        return gridManager.IsAdjacent(id , selectedBlocks[selectedBlocks.Count - 1].blockId);
    }

    public void UpdateGameModes(GameMode gm)
    {
        gameMode = gm;
        if (gm == GameMode.Endless)
        {
            responseData = dataPatcher.GetDataFromTextFile_Endless();
        }
        else
        {
            responseData = dataPatcher.GetDataFromTextFile();
        }
    }
}
