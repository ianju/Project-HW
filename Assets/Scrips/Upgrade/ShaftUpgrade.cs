using System.Collections;
using UnityEngine;


public class ShaftUpgrade : BaseUpgrade
{
    protected override void ExecuteUpgrade()
    {
        if (CurrentLevel % 10 == 0)
        {
            _shaft.CreateMiner();
        }

        foreach (ShaftMiner miner in _shaft.Miners)
        {
            miner.CollectCapacity *= CollectCapacityMultiplier;
            miner.CollectPerSecond *= CollectPerSecondMutiplier;
            if (CurrentLevel % 10 == 0)
            {
                miner._MoveSpeed *= MoveSpeedMultiplier;
            }
        }
    }
}