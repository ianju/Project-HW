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

    public int UpgradeAmount { get; set; }

    private Shaft _currentShaft;
    private ShaftUpgrade _currentShaftUpgrade;
    private int _currentActiveButton;


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
            _currentShaftUpgrade.Upgrade(UpgradeAmount);
            UpdateShaftPanelValues();
            RefreshUpgradeAmount();
        }
    }

    public void OpenCloseUpgradeContainer(bool status)
    {
        UpgradeX1();
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

    #region Upgrade Buttons
    public void UpgradeX1()
    {
        ActivateButton(0);
        UpgradeAmount = CanUpgradeManyTimes(1, _currentShaftUpgrade) ? 1 : 0;
        upgradeCost.text = GetUpgradeCost(1, _currentShaftUpgrade).ToString();
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

    public void UpgradeX10()
    {
        ActivateButton(1);
        UpgradeAmount = CanUpgradeManyTimes(10, _currentShaftUpgrade) ? 10 : 0;
        upgradeCost.text = GetUpgradeCost(10, _currentShaftUpgrade).ToString();
    }
    public void UpgradeX50()
    {
        ActivateButton(2);
        UpgradeAmount = CanUpgradeManyTimes(50, _currentShaftUpgrade) ? 50 : 0;
        upgradeCost.text = GetUpgradeCost(50, _currentShaftUpgrade).ToString();

    }
    public void UpgradeMax()
    {
        ActivateButton(3);
        int count = CalculateUpgradeCount(_currentShaftUpgrade);
        UpgradeAmount = count;
        upgradeCost.text = GetUpgradeCost(count, _currentShaftUpgrade).ToString();

    }

    private void ActivateButton(int buttonIndex)
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            upgradeButtons[i].GetComponent<Image>().color = buttonDisableColor;
        }
        _currentActiveButton = buttonIndex;
        upgradeButtons[buttonIndex].GetComponent<Image>().color = buttonEnableColor;
        upgradeButtons[buttonIndex].transform.DOPunchPosition(transform.localPosition + new Vector3(0f, -5f, 0f), 0.5f).Play();
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
                UpgradeX1();
                break;
            case 1:
                UpgradeX10();
                break;
            case 2:
                UpgradeX50();
                break;
            case 3:
                UpgradeMax();
                break;
        }
    }
    #endregion

    private void OnEnable()
    {
        ShaftUI.OnUpgradeRequest += ShaftUpgradeRequest;
    }

    private void OnDisable()
    {
        ShaftUI.OnUpgradeRequest -= ShaftUpgradeRequest;
    }

}
