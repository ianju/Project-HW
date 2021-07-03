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

    [Header("Upgrade Buttons")]
    [SerializeField] private  GameObject[] upgradeButtons;
    [SerializeField] private Color buttonDisableColor;
    [SerializeField] private Color buttonEnableColor;
     
    [Header("Stats")]
    [SerializeField] private GameObject[] stats;


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
    [SerializeField] private UpgradePanelInfo ElevatorMinerInfo;
    [SerializeField] private UpgradePanelInfo WarehouseMinerInfo;

    public int UpgradeAmount { get; set; }

    private Shaft _currentShaft;
    private int _minerCount;
    private int _currentActiveButton;
    private UpgradePanelInfo _currentPanelInfo;
    private BaseUpgrade _currentUpgrade;
    private BaseMiner _currentMiner;
    private void ShaftUpgradeRequest(Shaft shaft,ShaftUpgrade shaftUpgrade)
    {
        _minerCount = shaft.Miners.Count;
        _currentMiner = shaft.Miners[0];
        _currentPanelInfo = ShaftMinerInfo;
        _currentUpgrade = shaftUpgrade;
        UpdateUpgradeInfo();
        UpdatePanelValues();
        OpenCloseUpgradeContainer(true);
    }
    private void ElevatorUpgradeRequest(ElevatorUpgrade selectedUpgrade)
    {
        _minerCount = 1;
        _currentPanelInfo = ElevatorMinerInfo;
        _currentUpgrade = selectedUpgrade;
        _currentMiner = selectedUpgrade.GetComponent<Elevator>().Miner;
        UpdateUpgradeInfo();
        UpdatePanelValues();
        OpenCloseUpgradeContainer(true);
    }
    private void WarehouseUpgradeRequest(WarehouseUpgrade warehouseUpgrade)
    {
        _minerCount = warehouseUpgrade.GetComponent<Warehouse>().Miners.Count;
        _currentMiner = warehouseUpgrade.GetComponent<Warehouse>().Miners[0];
        _currentPanelInfo = WarehouseMinerInfo;
        _currentUpgrade = warehouseUpgrade;
        UpdateUpgradeInfo();
        UpdatePanelValues();
        OpenCloseUpgradeContainer(true);
    }
    private void UpdatePanelValues()
    {
        upgradeCost.text = _currentUpgrade.UpgradeCost.ToString();
        level.text = $"level {_currentUpgrade.CurrentLevel.ToString()}";
        progressBar.DOFillAmount(_currentUpgrade.CurrentLevel % 10 * 0.1f, 0.5f).Play();//GetNextBoostProgress()
        nextBoost.text = $"下次人数增加 - {_currentUpgrade.BoostLevel}";

        float nextMinerCount = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? 1 : 0;
        float nextMoveSpeed = (_currentUpgrade.CurrentLevel + 1) % 10 == 0 ? Math.Abs(_currentMiner._MoveSpeed * _currentUpgrade.MoveSpeedMultiplier - _currentMiner._MoveSpeed):0;
        float nextCollectPerSecond = Math.Abs(_currentMiner.CollectPerSecond * _currentUpgrade.CollectPerSecondMutiplier - _currentMiner.CollectPerSecond);
        float nextCollectCapacity = Math.Abs(_currentMiner.CollectCapacity * _currentUpgrade.CollectCapacityMultiplier - _currentMiner.CollectCapacity);
        if (_currentPanelInfo.location == Locations.Elevator)
        {
            stat1CurrentValue.text = $"{_currentMiner.CollectCapacity}";
            stat2CurrentValue.text = $"{_currentMiner._MoveSpeed}";
            stat3CurrentValue.text = $"{_currentMiner.CollectPerSecond}";

            stat1CurrentUpgrade.text = $"+{nextCollectCapacity}";
            stat2CurrentUpgrade.text = $"+{nextMoveSpeed}";
            stat3CurrentUpgrade.text = $"+{nextCollectPerSecond}";

        }
        else
        {
            stat1CurrentValue.text = $"{_minerCount}";
            stat2CurrentValue.text = $"{_currentMiner._MoveSpeed}";
            stat3CurrentValue.text = $"{_currentMiner.CollectPerSecond}";
            stat4CurrentValue.text = $"{_currentMiner.CollectCapacity}";
            stat1CurrentUpgrade.text = $"+{nextMinerCount}";
            stat2CurrentUpgrade.text = $"+{nextMoveSpeed}";
            stat3CurrentUpgrade.text = $"+{nextCollectPerSecond}";
            stat4CurrentUpgrade.text = $"+{nextCollectCapacity}";
        }

        
    }
    public void Upgrade()
    { 
        if (GoldManager.Instance.CurrentGold >= _currentUpgrade.UpgradeCost)
        {
            _currentUpgrade.Upgrade(UpgradeAmount);
            UpdatePanelValues();
            RefreshUpgradeAmount();
        }
    }

    public void OpenCloseUpgradeContainer(bool status)
    {
        UpgradeX1(false);
        upgradeContainer.SetActive(status);
    }
    private void UpdateUpgradeInfo()
    {
        if (_currentPanelInfo.location == Locations.Elevator)
        {
            stats[3].SetActive(false);
        }
        else
        {
            stats[3].SetActive(true);
        }
        panelTitle.text = _currentPanelInfo.PanelTitle;
        panelMinerImage.sprite = _currentPanelInfo.PanelMinerIcon;
        stat1Title.text = _currentPanelInfo.Stat1Title;
        stat2Title.text = _currentPanelInfo.Stat2Title;
        stat3Title.text = _currentPanelInfo.Stat3Title;
        stat4Title.text = _currentPanelInfo.Stat4Title;

        stat1Icon.sprite = _currentPanelInfo.Stat1Icon;
        stat2Icon.sprite = _currentPanelInfo.Stat2Icon;
        stat3Icon.sprite = _currentPanelInfo.Stat3Icon;
        stat4Icon.sprite = _currentPanelInfo.Stat4Icon;
    }

    #region Upgrade Buttons
    public void UpgradeX1(bool animateButton)
    {
        ActivateButton(0,animateButton);
        UpgradeAmount = CanUpgradeManyTimes(1, _currentUpgrade) ? 1 : 0;
        upgradeCost.text = GetUpgradeCost(1, _currentUpgrade).ToString();
    }
    private int CalculateUpgradeCount(BaseUpgrade upgrade)
    {
        if (upgrade == null) return 0;
        int count = 0;
        float currentGold = GoldManager.Instance.CurrentGold;
        float currentUpgradeCost = upgrade.UpgradeCost;
        if (GoldManager.Instance.CurrentGold >= currentUpgradeCost)
        {
            for (float i = currentGold; i >=0; i-=currentUpgradeCost)
            {
                count++;
                currentUpgradeCost *= upgrade.UpgradeCostMultiplier;
            }
        }
        return count;
    }

    private bool CanUpgradeManyTimes(int upgradeAmount,BaseUpgrade upgrade)
    {
        int count = CalculateUpgradeCount(upgrade);
        if (count>=upgradeAmount)
        {
            return true;
        }
        return false;
    }

    public void UpgradeX10(bool animateButton)
    {
        ActivateButton(1, animateButton);
        UpgradeAmount = CanUpgradeManyTimes(10, _currentUpgrade) ? 10 : 0;
        upgradeCost.text = GetUpgradeCost(10, _currentUpgrade).ToString();
    }
    public void UpgradeX50(bool animateButton)
    {
        ActivateButton(2, animateButton);
        UpgradeAmount = CanUpgradeManyTimes(50, _currentUpgrade) ? 50 : 0;
        upgradeCost.text = GetUpgradeCost(50, _currentUpgrade).ToString();

    }
    public void UpgradeMax(bool animateButton)
    {
        ActivateButton(3, animateButton);
        int count = CalculateUpgradeCount(_currentUpgrade);
        UpgradeAmount = count;
        upgradeCost.text = GetUpgradeCost(count, _currentUpgrade).ToString();

    }

    private void ActivateButton(int buttonIndex,bool animateButton)
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].GetComponent<Image>().color = buttonDisableColor;
        }
        _currentActiveButton = buttonIndex;
        upgradeButtons[buttonIndex].GetComponent<Image>().color = buttonEnableColor;

        if (animateButton)
        {
            upgradeButtons[buttonIndex].transform.DOKill(true);
            upgradeButtons[buttonIndex].transform.DOPunchPosition(transform.localPosition + new Vector3(0f, -5f, 0f), 0.5f).Play();
        }
    }

    private float GetUpgradeCost(int amount, BaseUpgrade upgrade)
    {
        float cost = 0f;
        float currentUpgradeCost = upgrade.UpgradeCost;
        for (int i = 0; i < amount; i++)
        {
            cost += currentUpgradeCost;
            currentUpgradeCost *= upgrade.UpgradeCostMultiplier;
        }
        return cost;
    }

    private void RefreshUpgradeAmount()
    {
        switch (_currentActiveButton)
        {
            case 0:
                UpgradeX1(false);
                break;
            case 1:
                UpgradeX10(false);
                break;
            case 2:
                UpgradeX50(false);
                break;
            case 3:
                UpgradeMax(false);
                break;
        }
    }
    #endregion



    private void OnEnable()
    {
        ShaftUI.OnUpgradeRequest += ShaftUpgradeRequest;
        ElevatorUI.OnUpgradeRequest += ElevatorUpgradeRequest;
        WarehouseUI.OnUpgradeRequest += WarehouseUpgradeRequest;
    }

    private void OnDisable()
    {
        ShaftUI.OnUpgradeRequest -= ShaftUpgradeRequest;
        ElevatorUI.OnUpgradeRequest -= ElevatorUpgradeRequest;
        WarehouseUI.OnUpgradeRequest -= WarehouseUpgradeRequest;
    }

}
