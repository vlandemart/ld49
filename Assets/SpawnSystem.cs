using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    private Transform[] spawnPoints;
    public GameObject myPrefab;

    public float minTime = 5f;
    public float maxTime = 7f;

    private bool waiting = false;


    void Start()
    {
        spawnPoints = GetCompNoRoot<Transform>(gameObject);
    }

    T[] GetCompNoRoot<T>(GameObject obj) where T : Component
    {
        List<T> tList = new List<T>();
        T[] comps = GetComponentsInChildren<T>();
        foreach (T comp in comps)
        {
            if (comp.gameObject.GetInstanceID() != obj.GetInstanceID())
            {
                tList.Add(comp);
            }
        }

        return tList.ToArray();
    }

    // Update is called once per frame

    void Update()
    {
        if (waiting)
        {
            return;
        }

        StartCoroutine(Spawn(Random.Range(minTime, maxTime)));
    }


    private IEnumerator Spawn(float time)
    {
        waiting = true;
        yield return new WaitForSeconds(time);
        int rndIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(myPrefab, spawnPoints[rndIndex].transform.position, Quaternion.identity);
        waiting = false;
    }

    private void OnDrawGizmos()
    {
        if (myPrefab)
            Handles.Label(transform.position, myPrefab.name);
    }
}