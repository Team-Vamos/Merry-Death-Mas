using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolingObj : MonoBehaviour
{
    public abstract void whenDestroy();

    public abstract Transform poolManager
    {
        get;
    }

    public abstract int poolChilds
    {
        get;
    }
}
