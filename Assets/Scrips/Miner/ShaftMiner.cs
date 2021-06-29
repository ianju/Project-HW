using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftMiner : BaseMiner
{
    private int walkAnimation = Animator.StringToHash("Walk");
    private int miningAnimation = Animator.StringToHash("Mine");

    protected override void CollectGold()
    {
        _animator.SetTrigger(miningAnimation);
        float collectTime = CollectCapacity / CollectPerSecond;
        StartCoroutine(IECollect(CollectCapacity, collectTime));
    }

    protected override void DepositGold()
    {
        ShaftDeposit.DepositGold(CurrentGold);
        CurrentGold = 0;
        ChangeGoal();
        Vector3 vc = new Vector3(MiningLocation.position.x, transform.position.y);
        RotateMiner(1);
        MoveMiner(vc);
    }

    protected override IEnumerator IECollect(float gold, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);
        CurrentGold = gold;
        ChangeGoal();
        Vector3 vc = new Vector3(DepositLocation.position.x, transform.position.y);
        RotateMiner(-1);
        MoveMiner(vc);
    }

    protected override void MoveMiner(Vector3 newPosition)
    {
        base.MoveMiner(newPosition);
        _animator.SetTrigger(walkAnimation);
    }
}
