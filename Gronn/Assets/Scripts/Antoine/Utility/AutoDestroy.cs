using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    float time;

    void Start()
    {
        Invoke("auto_destroy", time);
    }

    void auto_destroy()
    {
        Destroy(gameObject);
    }
}
