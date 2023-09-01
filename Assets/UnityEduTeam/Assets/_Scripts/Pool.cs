using UnityEngine;

public abstract class Pool : MonoBehaviour
{
    public abstract Transform Get(Vector3 position, Quaternion rotation);
    public abstract void Release(Transform element);
}
