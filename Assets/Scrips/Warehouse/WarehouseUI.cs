using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarehouseUI : MonoBehaviour
{
    public static Action<WarehouseUpgrade> OnUpgradeRequest;
    private WarehouseUpgrade _warehouseUpgrade;

    [SerializeField] private TextMeshProUGUI currentLevel;

    private void Start()
    {
        _warehouseUpgrade = GetComponent<WarehouseUpgrade>();
    }
    public void OpenWarehouseUpgradePanel()
    {
        OnUpgradeRequest?.Invoke(_warehouseUpgrade);
    }
    public void UpgradeCompleted(BaseUpgrade upgrade) 
    {
        if (_warehouseUpgrade == upgrade)
        {
            currentLevel.text = $"lv{upgrade.CurrentLevel}";
        }
    }
    private void OnEnable()
    {
        WarehouseUpgrade.OnUpgradeCompleted += UpgradeCompleted;
    }
    private void OnDisable()
    {
        WarehouseUpgrade.OnUpgradeCompleted -= UpgradeCompleted;
    }
}
