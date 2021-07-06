using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Warehouse : MonoBehaviour,MineLocation
{
    [Header("Prefab")]
    [SerializeField] private WarehouseMiner minerPrefab;
    [Header("Extras")]
    [SerializeField] private Deposit elevatorDeposit;
    [SerializeField] private Transform elevatorDepositLocation;
    [SerializeField] private Transform warehouseDepositLocation;
    [SerializeField] private TextMeshProUGUI TotalGoldText;
    [Header("Manager")]
    [SerializeField] private WarehouseWorkManager warehouseWorkManagerPrefab;
    [SerializeField] private Transform warehouseManagerPosition;


    private List<WarehouseMiner> _miners = new List<WarehouseMiner>();
    public List<WarehouseMiner> Miners => _miners;

    public WarehouseWorkManager WorkManager { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        AddMiner();
        AddManager();
    }

    public void AddMiner()
    {
        WarehouseMiner newMiner = Instantiate(minerPrefab, warehouseDepositLocation.position, Quaternion.identity);
        newMiner.ElevatorDeposit = elevatorDeposit;
        newMiner.ElevatorDepositLocation = new Vector3(elevatorDepositLocation.position.x, warehouseDepositLocation.position.y);
        newMiner.WarehouseLocation = new Vector3(warehouseDepositLocation.position.x, warehouseDepositLocation.position.y);

        if (_miners.Count > 0)
        {
            newMiner.CollectCapacity = _miners[0].CollectCapacity;
            newMiner.CollectPerSecond = _miners[0].CollectPerSecond;
            newMiner._MoveSpeed = _miners[0]._MoveSpeed;
        }
        _miners.Add(newMiner);
    }

    private void AddManager()
    {
        WorkManager = Instantiate(warehouseWorkManagerPrefab, warehouseManagerPosition.position, Quaternion.identity);
        WorkManager.transform.SetParent(transform);
        WorkManager.CurrentMineLocation = this;
    }

    public void ApplyManagerBoost()
    {
        switch (WorkManager.ManagerAssigned.BoostType)
        {
            case BoostType.步行速度:
                foreach (WarehouseMiner miner in _miners)
                {
                    WorkManagerController.Instance.RunMovementBoost(miner, WorkManager.ManagerAssigned.BoostDuration , WorkManager.ManagerAssigned.BoostValue);
                }
                break;
            case BoostType.装载速度:
                foreach (WarehouseMiner miner in _miners)
                {
                    WorkManagerController.Instance.RunLoadingBoost(miner, WorkManager.ManagerAssigned.BoostDuration , WorkManager.ManagerAssigned.BoostValue);
                }
                break;
        }
    }

}
