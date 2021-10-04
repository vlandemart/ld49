using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnGameObject : MonoBehaviour
{
    public GameObject myPrefab;

    public float time = 5f;

    private bool waiting = false;

    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            return;
        }

        StartCoroutine(Spawn(time));
    }

    private IEnumerator Spawn(float time)
    {
        waiting = true;
        yield return new WaitForSeconds(time);
        Instantiate(myPrefab, transform.position, Quaternion.identity);
        waiting = false;
    }

    private void OnDrawGizmos()
    {
        if (myPrefab)
            Handles.Label(transform.position, myPrefab.name);
    }
}
    