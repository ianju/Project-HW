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

    public int ShaftID { get; set; }
    public ShaftUI ShaftUI { get; set; }

    public Transform _MiningLocation => MiningLocation;
    public Transform _DepositLocation => DepositLocation;
    public Deposit ShaftDeposit { get; set; }

    private void Awake()
    {
        ShaftUI = GetComponent<ShaftUI>();
    }
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
        newMiner.transform.SetParent(transform);
        
    }

    private void CreateDeposit()
    {
        ShaftDeposit = Instantiate(DepositPrefab, DepositCreationLocation.position, Quaternion.identity);
        ShaftDeposit.transform.SetParent(transform);
    }
}
