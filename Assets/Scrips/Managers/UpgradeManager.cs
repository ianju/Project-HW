using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private GameObject upgradeContainer;

    private void ShaftUpgradeRequest()
    {
        OpenCloseUpgradeContainer(true);
    }

    public void OpenCloseUpgradeContainer(bool status)
    {
        upgradeContainer.SetActive(status);
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
