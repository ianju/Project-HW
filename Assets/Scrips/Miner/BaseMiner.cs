using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseMiner : MonoBehaviour,IClickable
{
    // [SerializeField] protected Transform MiningLocation;
    // [SerializeField] protected Transform DepositLocation;

    // [SerializeField] protected Deposit ShaftDeposit;
    public Shaft CurrentShaft { get; set; }

    [Header("Initial Values")]
    [SerializeField] private float InitialCollectCapacity;
    [SerializeField] private float InitialCollectPerSecond;
    [SerializeField] protected float MoveSpeed;

    public Vector3 DepositLocation => new Vector3(CurrentShaft._DepositLocation.position.x, transform.position.y);
    public Vector3 MiningLocation => new Vector3(CurrentShaft._MiningLocation.position.x, transform.position.y);

    public bool IsTimeToCollect { get; set; }
    public float CurrentGold { get; set; }
    public float CollectCapacity { get; set; }
    public float CollectPerSecond { get; set; }
    public bool MinerClicked { get; set; }
    protected Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        IsTimeToCollect = true;
        CollectCapacity = InitialCollectCapacity;
        CollectPerSecond = InitialCollectPerSecond;
    }

    public virtual void OnClick()
    {
        
    }

    protected virtual void MoveMiner(Vector3 newPosition)
    {
        transform.DOMove(newPosition, MoveSpeed)  
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                if (IsTimeToCollect)
                {
                    CollectGold();
                }
                else
                {
                    DepositGold();
                }
            })
            .Play();
    }

    protected virtual void CollectGold()
    {

    }

    protected virtual void DepositGold()
    {

    }

    protected void ChangeGoal() {
        IsTimeToCollect = !IsTimeToCollect;
    }

    protected void RotateMiner(int direction)
    {
        if (direction == 1)
        {

            transform.localScale = new Vector3(1, transform.localScale.y,transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);    
        }
    }

    protected virtual IEnumerator IECollect(float gold,float collectTime) 
    {
        yield return null;
    }

    protected virtual IEnumerator IEDeposit(float depositTime)
    {
        yield return null;
    }


}
