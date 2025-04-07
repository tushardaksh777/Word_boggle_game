using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVisualsController : MonoBehaviour
{
    public Action showSelection;
    public Action HideSelection;
    [SerializeField]
    private GameSelectionController gameSelectionController;
    [SerializeField]
    private PlayScreenUIController playScreenUIController;

    // Start is called before the first frame update

    private void Awake()
    {
        showSelection += OnSelectionStart;
        HideSelection += OnSelectionHide;
    }
    private void OnSelectionStart()
    {
        gameSelectionController.ShowSelectionScreen();
    }

    private void OnSelectionHide()
    {
        gameSelectionController.HideSelectionScreen();
    }

    public void ShowPlayUI()
    {
        playScreenUIController.ShowPlayScreen();
    }

    public void HidePlayUI()
    {
        playScreenUIController.HidePlayScreen();
    }


}
