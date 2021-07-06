using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public interface MineLocation
{
    void ApplyManagerBoost();
}

public class BaseWorkManager : MonoBehaviour
{

    [SerializeField] private GameObject boostButton;
    [SerializeField] private Image boostIcon;
    public MineLocation CurrentMineLocation { get; set; }

    public WorkManagerInfo ManagerAssigned { get; set; }


    public static Action<MineLocation> OnManagerClicked;

    private void Start()
    {
        HideBoostButton();
    }
    
    public void RunBoost()
    {
        CurrentMineLocation?.ApplyManagerBoost();
    }

    private void OnMouseDown()
    {
        OnManagerClicked?.Invoke(CurrentMineLocation);
    }

    private void HideBoostButton()
    {
        boostButton.SetActive(false);
    }

    public void SetupBoostButton()
    {
        if (ManagerAssigned != null)
        {
            boostButton.SetActive(true);
            boostIcon.sprite = ManagerAssigned.BoostIcon;
        }
    }
}
