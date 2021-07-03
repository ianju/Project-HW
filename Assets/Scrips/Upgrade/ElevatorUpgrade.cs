using System.Collections;
using UnityEngine;

public class ElevatorUpgrade : BaseUpgrade
{
    protected override void ExecuteUpgrade()
    {
        _elevator.Miner.CollectCapacity *= CollectCapacityMultiplier;
        _elevator.Miner.CollectPerSecond *= CollectPerSecondMutiplier;
        if ((CurrentLevel + 1)%10==0)
        {
            _elevator.Miner._MoveSpeed *= MoveSpeedMultiplier;
        }
    }
}
