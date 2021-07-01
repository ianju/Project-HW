using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseMiner : BaseMiner
{
    private int walkAnimation = Animator.StringToHash("Walk");
    public Deposit ElevatorDeposit { get; set; }
    public Vector3 ElevatorDepositLocation { get; set; }
    public Vector3 WarehouseLocation { get; set; }

    private void OnMouseDown()
    {
        
        if (!MinerClicked)
        {
            OnClick();
            MinerClicked = true;
        }
    }
    public override void OnClick()
    {
        RotateMiner(-1);
        MoveMiner(ElevatorDepositLocation);
    }
    protected override void MoveMiner(Vector3 newPosition)
    {
        base.MoveMiner(newPosition);
        _animator.SetBool(walkAnimation, true) ;
    }


    protected override void CollectGold()
    {
        if (!ElevatorDeposit.CanCollectGold)
        {
            RotateMiner(1);
            ChangeGoal();
            MoveMiner(WarehouseLocation);
            return;
        }
        _animator.SetBool(walkAnimation, false);
        float gold = ElevatorDeposit.CollectGold(this);
        float collectTime = gold / CollectPerSecond;
        StartCoroutine(IECollect(gold, collectTime));
    }

    protected override IEnumerator IECollect(float gold, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);

        ElevatorDeposit.RemoveGold(gold);
        CurrentGold = gold;
        ChangeGoal();
        RotateMiner(1);
        MoveMiner(WarehouseLocation);
        
    }

    protected override void DepositGold()
    {
        if (CurrentGold <= 0)
        {
            RotateMiner(-1);
            ChangeGoal();
            MoveMiner(ElevatorDepositLocation);
            return;
        }

        _animator.SetBool(walkAnimation, false);
        float depositTime = CurrentGold / CollectPerSecond;
        StartCoroutine(IEDeposit( depositTime));

        
    }

    protected override IEnumerator IEDeposit(float depositTime)
    {
        yield return new WaitForSeconds(depositTime);
        GoldManager.Instance.AddGold(CurrentGold);
        CurrentGold = 0;
        ChangeGoal();
        RotateMiner(-1);
        MoveMiner(ElevatorDepositLocation);
    }
}
