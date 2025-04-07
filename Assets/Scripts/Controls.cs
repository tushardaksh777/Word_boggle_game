using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    public GraphicRaycaster raycaster; // Assign in Inspector
    public EventSystem eventSystem;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    private bool blockInputs = false;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.blockInputs += GameManager_BlockInput;
    }

    // Update is called once per frame
    void Update()
    {
        if (!blockInputs && Input.touchCount > 0  && GameManager.Instance.gameStage == GameStage.Play)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = touch.position;

            pointerEventData = new PointerEventData(eventSystem) { position = touchPosition };

            raycastResults.Clear();
            raycaster.Raycast(pointerEventData, raycastResults);

            if (touch.phase == TouchPhase.Began)
            {
                DetectBlock();
            }
            if (touch.phase == TouchPhase.Moved)
            {
                DetectBlock();
            }
            if (touch.phase == TouchPhase.Ended)
            {
                OnRayFireStop();
            }
            //Debug.Log("Raycaster Length "+raycastResults.Count);
        }
    }

    private void GameManager_BlockInput(bool value)
    {
        blockInputs = value;
    }

    private void OnRayFireStop()
    {
        if (GameManager.Instance.selectedBlocks.Count > 0) {
            GameManager.Instance.onBlockDeselect.Invoke();
        }
        
    }


    private void DetectBlock()
    {
        foreach (RaycastResult raycastResult in raycastResults)
        {
            //Debug.Log("Ray started " + raycastResult.gameObject.name);
            if (raycastResult.gameObject.TryGetComponent<IBlockBehaviour>(out IBlockBehaviour blockHandler))
            {
                if (GameManager.Instance.selectedBlocks.Count == 0 && blockHandler.blockType != BlockType.Locked)
                {
                    GameManager.Instance.onBlockSelect.Invoke(blockHandler);
                    Debug.Log("Tile selected " + blockHandler.blockId);
                }
                else
                {
                    if (!CheckForDuplicate(blockHandler) && blockHandler.blockType != BlockType.Locked && GameManager.Instance.IsAdjacent(blockHandler.blockId))
                    {
                        GameManager.Instance.onBlockSelect.Invoke(blockHandler);
                        Debug.Log("Tile selected " + blockHandler.blockId);
                    }
                }
            }
        }
    }

    private bool CheckForDuplicate(IBlockBehaviour blockHandler)
    {
        List<IBlockBehaviour> selectedTile = GameManager.Instance.selectedBlocks;
        for (int i = 0; i < selectedTile.Count; i++)
        {
            if(selectedTile[i].blockId == blockHandler.blockId)
            {
                return true;
            }
        }
        return false;
    }
}
