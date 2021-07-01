using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI totalGoldText;
    private void Update()
    {
        totalGoldText.text = GoldManager.Instance.CurrentGold.ToString() ;
    }
}
