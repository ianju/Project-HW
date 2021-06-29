using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseMiner : MonoBehaviour
{
    [SerializeField] protected Transform MiningLocation;
    [SerializeField] protected Transform DepositLocation;
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected Deposit ShaftDeposit;


    [Header("Initial Values")]
    [SerializeField] private float InitialCollectCapacity;
    [SerializeField] private float InitialCollectPerSecond;

    public bool IsTimeToCollect { get; set; }
    public bool IsRunning { get; set; }
    public float CurrentGold { get; set; }
    public float CollectCapacity { get; set; }
    public float CollectPerSecond { get; set; }
    protected Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        IsTimeToCollect = true;
        CollectCapacity = InitialCollectCapacity;
        CollectPerSecond = InitialCollectPerSecond;
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
        IsRunning = true;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 vc = new Vector3(MiningLocation.position.x, transform.position.y);
        if (!IsRunning) {
            if (MiningLocation != null)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    MoveMiner(vc);

                }
            }
        }

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
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);    
        }
    }

    protected virtual IEnumerator IECollect(float gold,float collectTime) 
    {
        yield return null;
    }
}
