using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // singleton refrence
    public static SpawnManager instance { get; private set;}
    // game objects
    [SerializeField] GameObject target;

    // components
    private ObjectPooler objectPooler;
    // control variables
    [SerializeField] private float spawnRate = 10f;
    [SerializeField] private float startDelay = 0f;
    private float zRange = 5f;
    private float yRange = 1.5f;
    private float xRange = 11f;
    private int numberOfTargets;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = GetComponent<ObjectPooler>();
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        numberOfTargets = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnTarget()
    {
        float xSpawnPos = transform.position.x + Random.Range(-xRange, xRange);
        float zSpawnPos = transform.position.z + Random.Range(-zRange, zRange);
        float ySpawnPos = transform.position.y + Random.Range(-yRange, yRange);

        Vector3 spawnPos = new Vector3(xSpawnPos, ySpawnPos, zSpawnPos);
        GameObject target = objectPooler.GetPooledObject();
        target.transform.position= spawnPos;
        target.SetActive(true);
        numberOfTargets++;
        if (!GameManager.Instance.isStarted)
        {
            GameManager.Instance.StartCounting();
        }

    }


    // start spawning targets
    public void StartSpawning()
    {
        InvokeRepeating(nameof(SpawnTarget), startDelay, spawnRate);
    }   


    // stop spawning
    public void StopSpawning()
    {
        CancelInvoke();
    }
    
    public int GetNumberOfTargets()
    {
        return numberOfTargets;
    }

    public void SetSpawnRate(float value)
    {
        spawnRate = value;
    }
}
