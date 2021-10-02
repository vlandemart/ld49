using UnityEngine;

public class ThrowableObjectGetter : MonoBehaviour
{
    [SerializeField] private Rigidbody throwablePrefab;

    //maybe later, if it will stick as a mechanic
    public Rigidbody GetThrowableObject()
    {
        var newObject = Instantiate(throwablePrefab, transform.position, Quaternion.identity);
        return newObject;
    }
}