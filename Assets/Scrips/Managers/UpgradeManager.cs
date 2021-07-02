using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private GameObject upgradeContainer;
    [SerializeField] private Image panelMinerImage;
    [SerializeField] private TextMeshProUGUI panelTitle;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI nextBoost;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private Image progressBar;

    [Header("Stat Title")]
    [SerializeField] private TextMeshProUGUI stat1Title;
    [SerializeField] private TextMeshProUGUI stat2Title;
    [SerializeField] private TextMeshProUGUI stat3Title;
    [SerializeField] private TextMeshProUGUI stat4Title;

    [Header("Stat Values")]
    [SerializeField] private TextMeshProUGUI stat1CurrentValue;
    [SerializeField] private TextMeshProUGUI stat2CurrentValue;
    [SerializeField] private TextMeshProUGUI stat3CurrentValue;
    [SerializeField] private TextMeshProUGUI stat4CurrentValue;

    [Header("Stat Upgrade Values")]
    [SerializeField] private TextMeshProUGUI stat1CurrentUpgrade;
    [SerializeField] private TextMeshProUGUI stat2CurrentUpgrade;
    [SerializeField] private TextMeshProUGUI stat3CurrentUpgrade;
    [SerializeField] private TextMeshProUGUI stat4CurrentUpgrade;

    [Header("Stat Icon")]
    [SerializeField] private Image stat1Icon;
    [SerializeField] private Image stat2Icon;
    [SerializeField] private Image stat3Icon;
    [SerializeField] private Image stat4Icon;

    [Header("Panel Info")]
    [SerializeField] private UpgradePanelInfo ShaftMinerInfo;

    private Shaft _currentShaft;
    private ShaftUpgrade _currentShaftUpgrade;

    private void ShaftUpgradeRequest(Shaft shaft,ShaftUpgrade shaftUpgrade)
    {
        _currentShaft = shaft;
        _currentShaftUpgrade = shaftUpgrade;
        UpdateUpgradeInfo();
        UpdateShaftPanelValues();
        OpenCloseUpgradeContainer(true);
    }

    private void UpdateShaftPanelValues()
    {
        upgradeCost.text = _currentShaftUpgrade.UpgradeCost.ToString();
        level.text = $"level {_currentShaftUpgrade.CurrentLevel.ToString()}";
        progressBar.DOFillAmount(_currentShaftUpgrade.CurrentLevel % 10 * 0.1f, 0.5f).Play();//GetNextBoostProgress()
        nextBoost.text = $"下次人数增加 - {_currentShaftUpgrade.BoostLevel}";

        float nextMinerCount = (_currentShaftUpgrade.CurrentLevel + 1) % 10 == 0 ? 1 : 0;
        float nextMoveSpeed = (_currentShaftUpgrade.CurrentLevel + 1) % 10 == 0 ? Math.Abs(_currentShaft.Miners[0]._MoveSpeed * _currentShaftUpgrade.MoveSpeedMultiplier - _currentShaft.Miners[0]._MoveSpeed):0;
        float nextCollectPerSecond = Math.Abs(_currentShaft.Miners[0].CollectPerSecond * _currentShaftUpgrade.CollectPerSecondMutiplier - _currentShaft.Miners[0].CollectPerSecond);
        float nextCollectCapacity = Math.Abs(_currentShaft.Miners[0].CollectCapacity * _currentShaftUpgrade.CollectCapacityMultiplier - _currentShaft.Miners[0].CollectCapacity);

        stat1CurrentValue.text = $"{_currentShaft.Miners.Count}";
        stat2CurrentValue.text = $"{_currentShaft.Miners[0]._MoveSpeed}";
        stat3CurrentValue.text = $"{_currentShaft.Miners[0].CollectPerSecond}";
        stat4CurrentValue.text = $"{_currentShaft.Miners[0].CollectCapacity}";
        stat1CurrentUpgrade.text = $"+{nextMinerCount}";
        stat2CurrentUpgrade.text = $"+{nextMoveSpeed}";
        stat3CurrentUpgrade.text = $"+{nextCollectPerSecond}"; 
        stat4CurrentUpgrade.text = $"+{nextCollectCapacity}";
    }
    public void Upgrade()
    { 
        if (GoldManager.Instance.CurrentGold >= _currentShaftUpgrade.UpgradeCost)
        {
            _currentShaftUpgrade.Upgrade(1);
            UpdateShaftPanelValues();
        }
    }

    public void OpenCloseUpgradeContainer(bool status)
    {
        upgradeContainer.SetActive(status);
    }
    private void UpdateUpgradeInfo()
    {
        panelTitle.text = ShaftMinerInfo.PanelTitle;
        panelMinerImage.sprite = ShaftMinerInfo.PanelMinerIcon;
        stat1Title.text = ShaftMinerInfo.Stat1Title;
        stat2Title.text = ShaftMinerInfo.Stat2Title;
        stat3Title.text = ShaftMinerInfo.Stat3Title;
        stat4Title.text = ShaftMinerInfo.Stat4Title;

        stat1Icon.sprite = ShaftMinerInfo.Stat1Icon;
        stat2Icon.sprite = ShaftMinerInfo.Stat2Icon;
        stat3Icon.sprite = ShaftMinerInfo.Stat3Icon;
        stat4Icon.sprite = ShaftMinerInfo.Stat4Icon;
    }

    private void OnEnable()
    {
        ShaftUI.OnUpgradeRequest += ShaftUpgradeRequest;
    }

    private void OnDisable()
    {
        ShaftUI.OnUpgradeRequest -= ShaftUpgradeRequest;
    }
}
