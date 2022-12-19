using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTargets : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;
    [SerializeField] private float repeadRate;
    [SerializeField] private float startTime = 0;
    [SerializeField] private float spawnPosition = 0;

    private void Start()
    {
        repeadRate = Random.Range(3, 5);
        InvokeRepeating("InstantiateTargets", startTime, repeadRate);
    }


    private void InstantiateTargets()
    {
        switch (GameManager.Instance.Level)
        {
            case 1: 
                InstantiateTargetsIntoLevel(0);
                break;
            case 2:
                InstantiateTargetsIntoLevel(2);
                break;
            case 3:
                InstantiateTargetsIntoLevel(3);
                break;
            case 4:
                InstantiateTargetsIntoLevel(4);
                break;
            case 5:
                InstantiateTargetsIntoLevel(4);
                break;
            default:
                break;
        }
    }


    public void InstantiateTargetsIntoLevel(int level)
    {
        int randomTarget = Random.Range(0, level);
        GameObject target = Instantiate(targets[randomTarget], transform.position, Quaternion.identity);
        target.GetComponent<TargetsScript>().xSpawnDirection = spawnPosition;
    }
}
