using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShaftUI : MonoBehaviour
{
    public static Action<Shaft,ShaftUpgrade> OnUpgradeRequest;
    [SerializeField] private TextMeshProUGUI depositGold;
    [SerializeField] private TextMeshProUGUI shaftID;
    [SerializeField] private TextMeshProUGUI shaftLevel;
    [SerializeField] private TextMeshProUGUI newShaftCost;
    [SerializeField] private GameObject newShaftButton;
    private Shaft _shaft;
    private ShaftUpgrade _shaftUpgrade;
    // Start is called before the first frame update
    void Awake()
    {
        _shaft =transform.GetComponent<Shaft>();
        _shaftUpgrade = transform.GetComponent<ShaftUpgrade>();
    }

    // Update is called once per frame
    void Update()
    {
        depositGold.text = _shaft.ShaftDeposit.CurrentGold.ToString();
    }

    public void AddShaft()
    {
        if (GoldManager.Instance.CurrentGold>=ShaftManager.Instance.ShaftCost)
        {
            GoldManager.Instance.RemoveGold(ShaftManager.Instance.ShaftCost);
            ShaftManager.Instance.AddShaft();
            newShaftButton.SetActive(false);
        }
        
    }

    public void SetShaftUI(int ID) 
    {
        _shaft.ShaftID = ID;
        shaftID.text = (ID + 1).ToString();
    }

    public void SetNewShaftCost(float newCost)
    {
        newShaftCost.text = newCost.ToString();
    }

    public void OpenUpgradeContainer()
    {
        OnUpgradeRequest?.Invoke(_shaft, _shaftUpgrade);
    }

    public void UpgradeCompleted(BaseUpgrade shaftUpgrade)
    {
        if (_shaftUpgrade == shaftUpgrade)
        {
            shaftLevel.text = $"lv{shaftUpgrade.CurrentLevel}";
        }
    }

    private void OnEnable()
    {
        ShaftUpgrade.OnUpgradeCompleted += UpgradeCompleted;
    }

    private void OnDisable()
    {
        ShaftUpgrade.OnUpgradeCompleted -= UpgradeCompleted;

    }
}
