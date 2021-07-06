using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WorkManagerCard : MonoBehaviour
{
    public static Action<WorkManagerCard> OnAssignRequest;

    [SerializeField] private Image boostIcon;
    [SerializeField] private TextMeshProUGUI managerType;
    [SerializeField] private TextMeshProUGUI boostDuration;
    [SerializeField] private TextMeshProUGUI boostDescription;

    public WorkManagerInfo ManagerInfoAssigned { get; set; }

    public void SetupWorkManagerCard(WorkManagerInfo managerInfo)
    {
        ManagerInfoAssigned = managerInfo;
        boostIcon.sprite = managerInfo.BoostIcon;
        managerType.text = managerInfo.managerType.ToString();
        managerType.color = managerInfo.levelColor;
        boostDuration.text = $"持续时间:{managerInfo.BoostDuration}分钟";
        boostDescription.text = $"{managerInfo.BoostDescription}";
    }

    public void AssignManager()
    {
        OnAssignRequest?.Invoke(this);
        //gameObject.SetActive(false);
    }
}
