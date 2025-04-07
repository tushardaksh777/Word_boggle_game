using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelectionController : MonoBehaviour
{
    public GameObject BG;
    public GameObject EndlessButton;
    public GameObject LevelsButton;
    public GameObject Text;

    private void Awake()
    {
        HideSelectionScreen();
    }
    // Start is called before the first frame update
    public void ShowSelectionScreen()
    {
        BG.SetActive(true);
        EndlessButton.SetActive(true);
        LevelsButton.SetActive(true);
        Text.SetActive(true);
    }

    public void HideSelectionScreen()
    {
        BG.SetActive(false);
        EndlessButton.SetActive(false);
        LevelsButton.SetActive(false);
        Text.SetActive(false);
    }

    public void OnEndlessButtonClicked()
    {
        GameManager.Instance.UpdateGameModes(GameMode.Endless);
        GameManager.Instance.UpdateGameStages(GameStage.Play);
    }

    public void OnLevelsButtonClicked()
    {
        GameManager.Instance.UpdateGameModes(GameMode.Levels);
        GameManager.Instance.UpdateGameStages(GameStage.Play);
    }
}
