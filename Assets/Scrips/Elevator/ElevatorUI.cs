using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class ElevatorUI : MonoBehaviour
{
    public static Action<ElevatorUpgrade> OnUpgradeRequest;

    [SerializeField] private TextMeshProUGUI elevatorDepositGoldText;
    [SerializeField] private TextMeshProUGUI currentLevel;

    private Elevator _elevator;
    private ElevatorUpgrade _elevatorUpgrade;
    // Start is called before the first frame update
    void Start()
    {
        _elevator = GetComponent<Elevator>();
        _elevatorUpgrade = GetComponent<ElevatorUpgrade>();

    }

    // Update is called once per frame
    void Update()
    {
        elevatorDepositGoldText.text = _elevator.ElevatorDeposit.CurrentGold.ToString();
    }

    public void OpenElevatorUpgrade()
    {
        OnUpgradeRequest?.Invoke(_elevatorUpgrade);
    }

    public void UpgradeCompleted(BaseUpgrade upgrade)
    {
        if (_elevatorUpgrade == upgrade)
        {
            currentLevel.text =$"lv{upgrade.CurrentLevel}" ;
        }
    }

    private void OnEnable()
    {
        ElevatorUpgrade.OnUpgradeCompleted += UpgradeCompleted;
    }
    private void OnDisable()
    {
        ElevatorUpgrade.OnUpgradeCompleted -= UpgradeCompleted;
    }
}
