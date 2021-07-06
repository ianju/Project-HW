using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;

    public Vector3 SpawnPosition { get; set; }

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.position = new Vector3(SpawnPosition.x, SpawnPosition.y+Random.Range(-0.5f,0.5f));
    }
}
