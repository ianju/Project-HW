using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseUpgrade : BaseUpgrade
{
    protected override void ExecuteUpgrade()
    {
        if (CurrentLevel  % 10 == 0)
        {
            _warehouse.AddMiner();
        }
        foreach (WarehouseMiner miner in _warehouse.Miners)
        {
            miner.CollectCapacity *= CollectCapacityMultiplier;
            miner.CollectPerSecond *= CollectPerSecondMutiplier;
            if (CurrentLevel  % 10 == 0)
            {
                miner._MoveSpeed *= MoveSpeedMultiplier;
            }
        }
    }
}
