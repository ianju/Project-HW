using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaft : MonoBehaviour
{

    [Header("Prefab")]
    [SerializeField] private ShaftMiner MinerPrefab;
    [SerializeField] private Deposit DepositPrefab;
    [Header("Locations")]
    [SerializeField] private Transform MiningLocation;
    [SerializeField] private Transform DepositLocation;
    [SerializeField] private Transform DepositCreationLocation;
    // Start is called before the first frame update
    void Start()
    {
        CreateMiner();
        CreateDeposit();
    }

    // Update is called once per frame
    private void CreateMiner()
    {
        ShaftMiner newMiner =  Instantiate(MinerPrefab, DepositLocation.position, Quaternion.identity);
        newMiner.CurrentShaft = this;
        newMiner.transform.SetParent(DepositLocation);
        
    }

    private void CreateDeposit()
    {
        Instantiate(DepositPrefab, DepositCreationLocation.position, Quaternion.identity);
    }
}
