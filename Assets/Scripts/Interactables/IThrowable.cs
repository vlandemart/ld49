using UnityEngine;

public interface IThrowable
{
    void Take();

    void Throw(Vector3 direction);
}