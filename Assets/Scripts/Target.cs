using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float timeToLive;

    // setting time to live from game manager
    public void SetTimeToLive (float newTTL)
    {
        // not used yet
        timeToLive = newTTL;
    }

    
    void OnEnable()
    {
        Debug.Log("Enabled this target");
        StartCoroutine(Dectivate());
    }

    // make object disapear after timeToLive has passed
    IEnumerator Dectivate()
    {
        yield return new WaitForSeconds(timeToLive);
        gameObject.SetActive(false);
    }
}
