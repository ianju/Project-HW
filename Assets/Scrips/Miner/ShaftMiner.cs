using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftMiner : BaseMiner
{

    private int walkAnimation = Animator.StringToHash("Walk");
    private int miningAnimation = Animator.StringToHash("Mine");

    public bool MinerClicked { get; set; }

    protected override void CollectGold()
    {
        _animator.SetTrigger(miningAnimation);
        float collectTime = CollectCapacity / CollectPerSecond;
        StartCoroutine(IECollect(CollectCapacity, collectTime));
    }

    protected override void DepositGold()
    {
        CurrentShaft.ShaftDeposit.DepositGold(CurrentGold);
        CurrentGold = 0;
        ChangeGoal();
        RotateMiner(1);
        MoveMiner(MiningLocation);
    }

    protected override IEnumerator IECollect(float gold, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);
        CurrentGold = gold;
        ChangeGoal();
        RotateMiner(-1);
        MoveMiner(DepositLocation);
    }

    protected override void MoveMiner(Vector3 newPosition)
    {
        base.MoveMiner(newPosition);
        _animator.SetTrigger(walkAnimation);
    }

    private void OnMouseDown()
    {
        if (!MinerClicked) 
        {
            OnClick();
            MinerClicked = true  ;
        }
    }

    public override void OnClick()
    {
        MoveMiner(MiningLocation);
    }
}
