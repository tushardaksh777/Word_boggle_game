using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBlockBehaviour
{
    public BlockType blockType { get; }
    public int blockId { get; }
    public string letter { get; }
    public void OnSelect();
    public void OnDeselect();

    public void OnUpdateType(BlockType blockType);
}
