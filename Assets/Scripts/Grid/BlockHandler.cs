using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class BlockHandler : MonoBehaviour , IBlockBehaviour
{
    public int id;
    public TextMeshProUGUI letter_text;
    private string letter;
    private BlockType type;
    [SerializeField]
    private GameObject Bonus;

    public Transform rootObject;

    [SerializeField]
    private GameObject locked;
    int IBlockBehaviour.blockId { get => id; }
    string IBlockBehaviour.letter { get => letter; }

    BlockType IBlockBehaviour.blockType { get => type; }
    private void Start()
    {
        GameManager.Instance.onDataRecevied += GameManager_SetLetter;
    }
    public void GameManager_SetLetter(DataModel data)
    {
        letter = data.gridData[id].letter;
        letter_text.text = letter;

        switch (data.gridData[id].tileType)
        {
            case 0:
                type = BlockType.Normal;
                UpdateBlockType(BlockType.Normal);
                break;
            case 1:
                type = BlockType.Bonus;
                UpdateBlockType(BlockType.Bonus);
                break;
            case 2:
                type = BlockType.Locked;
                UpdateBlockType(BlockType.Locked);
                break;
        }
        
    }

    private void OnSelect()
    {
        Debug.Log("Block Selected");
        gameObject.GetComponent<Image>().color = Color.green;
    }

    private void OnDeselect()
    {
        Debug.Log("Block DeSelect");
        gameObject.GetComponent<Image>().color = Color.white;
    }

    void IBlockBehaviour.OnSelect()
    {
        OnSelect();
    }

    void IBlockBehaviour.OnDeselect()
    {
        OnDeselect();
    }

    public void OnUpdateType(BlockType blockType)
    {
        UpdateBlockType(blockType);
    }

    private void UpdateBlockType(BlockType blockType) {
        switch (blockType)
        {
            case BlockType.Normal:
                type = BlockType.Normal;
                SwitchToNormal();
                break;
            case BlockType.Bonus:
                type = BlockType.Bonus;
                SwitchToBonus();
                break;
            case BlockType.Locked:
                type = BlockType.Locked;
                SwitchTolocked();
                break;
        }
    }

    private void SwitchToNormal()
    {
        Bonus.SetActive(false);
        locked.SetActive(false);
    }
    private void SwitchToBonus()
    {
        Bonus.SetActive(true);
        locked.SetActive(false);
    }
    private void SwitchTolocked()
    {
        Bonus.SetActive(false);
        locked.SetActive(true);
    }
    public void ResetPosition(float time)
    {
        RectTransform rootRect = rootObject.GetComponent<RectTransform>();
        StartCoroutine(MoveToPosition(rootRect, new Vector2(0, 0), time));
    }
    public IEnumerator MoveToPosition(RectTransform rootRect, Vector2 targetPosition, float time)
    {
        Vector2 startPos = rootRect.anchoredPosition;
        float elapsed = 0f;

        while (rootRect.anchoredPosition.y != 0)
        {
            rootRect.anchoredPosition = Vector2.Lerp(startPos, targetPosition, elapsed / time);
            elapsed = elapsed + Time.deltaTime;
            // Wait for next frame
            yield return null;
        }
        
        rootRect.anchoredPosition = targetPosition; // Snap to final position
    }
    
    private string GetRandomLetter()
    {
        int a = UnityEngine.Random.Range(65, 91);
        return ((char)a).ToString();
    }
    public void UpdateLetter()
    {
        letter = GetRandomLetter();
        letter_text.text = letter;
    }
}
