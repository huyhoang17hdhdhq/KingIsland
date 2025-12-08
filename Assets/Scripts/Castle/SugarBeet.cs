using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarBeet : CropGrowController
{
    protected override ItemType RewardType => ItemType.Sugar;

    protected override void Harvest()
    {
        base.Harvest();
        Debug.Log("Củ cải đường đã được thu hoạch");
    }
}
