using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlaneBehaviour : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject SpawnPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            Instantiate(playerPrefab, SpawnPoint.transform.position, Quaternion.identity);
        }
    }
}
