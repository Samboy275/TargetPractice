using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // singleton
    public static ObjectPooler Instance { get; private set;}
    // Game Objects
    [SerializeField] GameObject objectToPool;
    private List<GameObject> objectsPool;
    [SerializeField] private int numOfObjects;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        objectsPool = new List<GameObject>();
        for (int i = 0; i < numOfObjects; i++)
        {
            Vector3 spawnPos = new Vector3(transform.position.x + (i * 5), transform.position.y + 20, 0); 
            GameObject target = Instantiate(objectToPool, spawnPos, objectToPool.transform.rotation);
            target.SetActive(false);
            objectsPool.Add(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTimeToLiveForTargets(float value)
    {
        foreach (GameObject target in objectsPool)
        {
            target.GetComponent<Target>().SetTimeToLive(value);
        }
    }
    public GameObject GetPooledObject()
    {
        GameObject objectToGet = null;
        foreach (GameObject target in objectsPool)
        {
            if (target.activeInHierarchy == false)
            {
                objectToGet =  target;
                break;
            }
        }
        return objectToGet;
    }
}
