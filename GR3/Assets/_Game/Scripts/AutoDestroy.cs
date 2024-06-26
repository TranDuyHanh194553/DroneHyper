using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float existTime = 3f;
    void Start()
    {
        Destroy(gameObject, existTime); 
    }
}
