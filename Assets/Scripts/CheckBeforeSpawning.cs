using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckBeforeSpawning : MonoBehaviour
{
    [ExecuteInEditMode]
    private void OnValidate()
    {
        if (Physics.OverlapSphere(transform.position, 2) is Collider[] s)
        {
            if(s.Length > 0)
            {
                DestroyImmediate(gameObject);
            }
        }
    }
    
}
