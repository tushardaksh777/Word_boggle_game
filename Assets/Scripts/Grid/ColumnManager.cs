using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnManager : MonoBehaviour
{
    public int columnId = 0;
    public GameObject AboveBlockParent;
    public List<GameObject> Gridblocks;
    public List<BlockHandler> blockHandlers = new List<BlockHandler>();

    public List<BlockHandler> removableBlocks = new List<BlockHandler>();

    public List<BlockHandler> removedBlocks = new List<BlockHandler>();

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.gridManager.startRemovingBlock += StartRemovingblocks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIds(int length , int columnlength)
    {
        columnId = length;
        for (int i = 0; i < blockHandlers.Count; i++)
        {
            blockHandlers[i].id = length + (i*columnlength);
        }
    }

    public void AddRemovableBlocks(int index)
    {
        removableBlocks.Add(blockHandlers[index]);
    }

    public void StartRemovingblocks()
    {
        if (removableBlocks.Count == 0)
            return;

        removableBlocks.Sort((item1, item2)=>{ return item1.id.CompareTo(item2.id); });
        int firstBlockRow = removableBlocks[0].id / blockHandlers.Count;  //Here i am getting row in which block present that is rowIndex = id/rowCount;
        int lastBlockRow = removableBlocks[removableBlocks.Count - 1].id / blockHandlers.Count;

        foreach (var item in removableBlocks)
        {
            item.rootObject.gameObject.SetActive(false);
            item.rootObject.SetParent(AboveBlockParent.transform);
            item.rootObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        for (int i = 0; i < removableBlocks.Count; i++)
        {
            int indextoRemove = getBlockIndexUsingId(removableBlocks[i].id);
            if(indextoRemove > -1)
            {
                blockHandlers.RemoveAt(indextoRemove);
            }
            removedBlocks.Add(removableBlocks[i]);
        }
        removableBlocks.Clear();
        if (firstBlockRow > 0)
        {
            for (int i = firstBlockRow - 1; i >= 0; i--)
            {
                //Shifting Non - Removable Blocks;
                blockHandlers[i].rootObject.SetParent(Gridblocks[lastBlockRow].transform);
                blockHandlers[i].ResetPosition(1f);
                blockHandlers[i].id = columnId + lastBlockRow * 4;
                lastBlockRow -= 1;
                
            }

        }
        StartCoroutine(AddNewBlocks(lastBlockRow));

    }

    private int getBlockIndexUsingId(int id)
    {
        for (int i = 0; i < blockHandlers.Count; i++)
        {
            if (blockHandlers[i].id == id)
            {
                return i;
            }
        }

        return -1;
    }

    IEnumerator AddNewBlocks(int lastBlockRow)
    {
        yield return new WaitForSeconds(0.4f);
        for (int i = lastBlockRow; i >= 0; i--)
        {
            BlockHandler block = removedBlocks[removedBlocks.Count - 1];
            block.rootObject.gameObject.SetActive(true);
            block.rootObject.SetParent(Gridblocks[lastBlockRow].transform);
            block.id = columnId + lastBlockRow * 4;
            block.UpdateLetter();
            block.ResetPosition(0.8f);
            blockHandlers.Add(block);
            removedBlocks.RemoveAt(removedBlocks.Count - 1);
            lastBlockRow -= 1;
        }
        removedBlocks.Clear();
        SortColumns();
        GameManager.Instance.blockInputs.Invoke(false);
        //GameManager.Instance.gridManager.updateIds.Invoke();
        
    }

    private void SortColumns()
    {
        blockHandlers.Sort((item1, item2) => { return item1.id.CompareTo(item2.id); });
    }
    
}