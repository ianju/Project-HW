using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deposit : MonoBehaviour
{
    public float CurrentGold { get; set; }
    public void DepositGold(float amount) {
        CurrentGold += amount;
    }
}
