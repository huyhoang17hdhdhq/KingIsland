using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatFiled : CropGrowController
{
    protected override ItemType RewardType => ItemType.Filed;

    protected override void Harvest()
    {
        MusicManager.Instance.FarmFieldSound();
        base.Harvest();
        Debug.Log("Củ cải đường đã được thu hoạch");
    }
}
