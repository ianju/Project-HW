using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkManagerController : Singleton<WorkManagerController>
{
    [SerializeField] private GameObject managerCardPrefab;
    [SerializeField] private Transform managerPanelContainer;
    [SerializeField] private List<WorkManagerInfo> availableManagers;

    [Header("Assigned Manager Card")]
    [SerializeField] private GameObject managerPanel;
    [SerializeField] private GameObject assignManagerCard;
    [SerializeField] private Image boostIcon;
    [SerializeField] private TextMeshProUGUI managerType;
    [SerializeField] private TextMeshProUGUI boostDuration;
    [SerializeField] private TextMeshProUGUI boostDescription;

    [SerializeField] private float initManagerCost = 200;
    [SerializeField] private float managerCostMultiplier = 200;
    [SerializeField] private TextMeshProUGUI hireCost;

    
    public BaseWorkManager CurrentWorkManager { get; set; }

    public float CurrentManagerCost { get; set; }

    private List<WorkManagerCard> _workManagerCardAssigned = new List<WorkManagerCard>();


    private void Start()
    {
        CurrentManagerCost = initManagerCost;
    }
    private void Update()
    {
        hireCost.text = CurrentManagerCost.ToString();
    }

    public void OpenCloseManagerPanel(bool status)
    {
        managerPanel.SetActive(status);
    }

    #region Boost
    public void RunMovementBoost(BaseMiner miner, float duration, float value)
    {
        StartCoroutine(IEMovementBoost(miner, duration, value));
    }
    public void RunLoadingBoost(BaseMiner miner, float duration, float value)
    {
        StartCoroutine(IELoadingBoost(miner, duration, value));
    }
    private IEnumerator IEMovementBoost(BaseMiner miner, float duration,float value)
    {
        if (!miner.Boosted) 
        {
            float startMoveSpeed = miner._MoveSpeed;
            miner._MoveSpeed *= value;
            miner.Boosted = true;
            yield return new WaitForSeconds(duration * 60);
            miner._MoveSpeed = startMoveSpeed;
            miner.Boosted = false;
        }
        
    }

    private IEnumerator IELoadingBoost(BaseMiner miner, float duration, float value)
    {
        if (!miner.Boosted)
        {
            float startLoadingSpeed = miner.CollectPerSecond;
            miner.CollectPerSecond *= value;
            miner.Boosted = true;
            yield return new WaitForSeconds(duration * 60);
            miner.CollectPerSecond = startLoadingSpeed;
            miner.Boosted = false;

        }
    }
    #endregion

    public void HireManager()
    {
        if (availableManagers.Count > 0 && GoldManager.Instance.CurrentGold>=CurrentManagerCost)
        {

            GameObject managerCardGO = Instantiate(managerCardPrefab, managerPanelContainer);
            WorkManagerCard managerCard = managerCardGO.GetComponent<WorkManagerCard>();
            int randomManagerIndex = Random.Range(0, availableManagers.Count);
        
        
            WorkManagerInfo managerInfo = availableManagers[randomManagerIndex];
            managerCard.SetupWorkManagerCard(managerInfo);
            availableManagers.RemoveAt(randomManagerIndex);

            GoldManager.Instance.RemoveGold(CurrentManagerCost);
            CurrentManagerCost *= managerCostMultiplier;
        }
        
    }

    private void UpdateAssignManagerCard()
    {
        if (CurrentWorkManager.ManagerAssigned != null)
        {
            assignManagerCard.SetActive(true);
            boostIcon.sprite = CurrentWorkManager.ManagerAssigned.BoostIcon;
            managerType.text= CurrentWorkManager.ManagerAssigned.managerType.ToString();
            managerType.color = CurrentWorkManager.ManagerAssigned.levelColor;
            boostDuration.text = $"持续时间: {CurrentWorkManager.ManagerAssigned.BoostDuration.ToString()}分钟"; 
            boostDescription.text = $"{CurrentWorkManager.ManagerAssigned.BoostDescription.ToString()}";


        }else
        {
            assignManagerCard.SetActive(false);
        }
    }

    private void ManagerClicked(MineLocation mineLocation)
    {
        if (mineLocation is Shaft shaft)
        {
            CurrentWorkManager = shaft.WorkManager;
        }
        else if(mineLocation is Elevator elevator)
        {
            CurrentWorkManager = elevator.WorkManager;
        }
        else if (mineLocation is Warehouse warehouse)
        {
            CurrentWorkManager = warehouse.WorkManager;
        }

        if (CurrentWorkManager.ManagerAssigned == null)
        {
            assignManagerCard.SetActive(false);
        }
        else
        {
            assignManagerCard.SetActive(true);
        }
        UpdateAssignManagerCard();
        OpenCloseManagerPanel(true);
    }

    private void WorkManagerCardAssigned(WorkManagerCard workManagerCard)
    {
        if (CurrentWorkManager.ManagerAssigned == null)
        {
            _workManagerCardAssigned.Add(workManagerCard);
            CurrentWorkManager.ManagerAssigned = workManagerCard.ManagerInfoAssigned;
            CurrentWorkManager.SetupBoostButton();
            workManagerCard.gameObject.SetActive(false);
            UpdateAssignManagerCard();
        }
    }


    public void UnassignManager()
    {
        RestoreManagerCardAssigned();
        UpdateAssignManagerCard();
    }

    private void RestoreManagerCardAssigned()
    {
        WorkManagerCard managerCardAssigned = null;
        for (int i = 0; i < _workManagerCardAssigned.Count; i++)
        {
            if (CurrentWorkManager.ManagerAssigned == _workManagerCardAssigned[i].ManagerInfoAssigned)
            {
                managerCardAssigned = _workManagerCardAssigned[i];

            }
        }
        if (managerCardAssigned != null)
        {
            managerCardAssigned.gameObject.SetActive(true);
            _workManagerCardAssigned.Remove(managerCardAssigned);
            CurrentWorkManager.ManagerAssigned = null;
        }
    }

    private void OnEnable()
    {
        BaseWorkManager.OnManagerClicked += ManagerClicked;
        WorkManagerCard.OnAssignRequest += WorkManagerCardAssigned;
    }
    private void OnDisable()
    {
        BaseWorkManager.OnManagerClicked -= ManagerClicked;
        WorkManagerCard.OnAssignRequest -= WorkManagerCardAssigned;

    }

}
