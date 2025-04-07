using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayScreenUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject BG;
    [SerializeField]
    private GameObject Objective_text;
    [SerializeField]
    private TextMeshProUGUI Objective;
    [SerializeField]
    private TextMeshProUGUI Word_value;
    [SerializeField]
    private GameObject ScoreText;
    [SerializeField]
    private TextMeshProUGUI Score_value;

    [SerializeField]
    private GameObject resetButton;
    // Start is called before the first frame update

    private void Awake()
    {
        HidePlayScreen();
    }
    void Start()
    {
        GameManager.Instance.OnUpdateWord += GameManager_UpdateWord;
        GameManager.Instance.OnUpdateScore += GameManager_UpdateScore;
    }

    public void ShowPlayScreen()
    {
        BG.SetActive(true);
        Objective_text.SetActive(true);
        Objective.gameObject.SetActive(true);
        Word_value.gameObject.SetActive(true);
        ScoreText.SetActive(true);
        Score_value.gameObject.SetActive(true);
        resetButton.SetActive(true);
    }
    public void HidePlayScreen()
    {
        BG.SetActive(false);
        Objective_text.SetActive(false);
        Objective.gameObject.SetActive(false);
        Word_value.gameObject.SetActive(false);
        ScoreText.SetActive(false);
        Score_value.gameObject.SetActive(false);
        resetButton.SetActive(false);
    }


    private void GameManager_UpdateWord()
    {
        Word_value.text = GameManager.Instance.Word;
    }

    private void GameManager_UpdateScore()
    {
        Score_value.text = GameManager.Instance.score.ToString();
    }

    public void OnReset()
    {
        GameManager.Instance.UpdateGameStages(GameStage.SelectGameModes);
    }

}
