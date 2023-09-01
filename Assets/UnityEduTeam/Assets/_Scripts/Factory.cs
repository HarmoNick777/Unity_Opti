using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Factory : MonoBehaviour
{
    public abstract Transform Generate(Vector3 position, Quaternion rotation);
}
