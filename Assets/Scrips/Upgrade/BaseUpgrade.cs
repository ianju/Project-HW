using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BaseUpgrade : MonoBehaviour
{
    public static Action<BaseUpgrade> OnUpgradeCompleted;

    [Header("Upgrade")]
    [SerializeField] private float collectCapacityMultiplier = 2;
    [SerializeField] private float collectPerSecondMultiplier = 2;
    [SerializeField] private float moveSpeedMultiplier = 2;

    [SerializeField] private float initialUpgradeCost = 600;
    [SerializeField] private float upgradeCostMultiplier = 2;

    
    public int CurrentLevel { get; set; }
    public float UpgradeCost { get; set; }
    public int BoostLevel { get; set; }

    public float UpgradeCostMultiplier => upgradeCostMultiplier;
    public float CollectCapacityMultiplier => collectCapacityMultiplier;
    public float CollectPerSecondMutiplier => collectPerSecondMultiplier;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;

    protected Shaft _shaft;

    private int _currentNextBoostLevel=1;
    private int _nextBoostResetValue = 1;

    private void Start()
    {
        _shaft = GetComponent<Shaft>();
        CurrentLevel = 1;
        UpgradeCost = initialUpgradeCost;
        BoostLevel = 10;
        
    }

    public void Upgrade(int amount)
    {
        if (amount > 0)
        {
            for (int i = 0; i < amount; i++)
            {
                UPgradeCompleted();
                ExecuteUpgrade();
            }
        }
    }

    private void UPgradeCompleted()
    {
        CurrentLevel++;
        GoldManager.Instance.RemoveGold(UpgradeCost);
        UpgradeCost *= upgradeCostMultiplier;
        UpdateNextBoostLevel();
        OnUpgradeCompleted?.Invoke(this);
    }

    protected virtual void ExecuteUpgrade()
    {

    }

    protected void UpdateNextBoostLevel()
    {
        _currentNextBoostLevel++;
        _nextBoostResetValue++;
        if (_currentNextBoostLevel == BoostLevel)
        {
            _nextBoostResetValue = 1;
            BoostLevel += 10;
        }
    }

    public float GetNextBoostProgress()
    {
        return (float)_nextBoostResetValue / 10;
    }
}
