using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private GameObject CloudPrefab;
    [SerializeField] private Transform spawnPos;

    private void Start()
    {
        SpawnCloud();
    }

    private void SpawnCloud()
    {
        GameObject CloudGO= Instantiate(CloudPrefab, spawnPos.position, Quaternion.identity);
        Cloud cloud = CloudGO.GetComponent<Cloud>();
        cloud.SpawnPosition = spawnPos.position;
    }
}
