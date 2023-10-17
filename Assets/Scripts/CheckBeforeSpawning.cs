using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckBeforeSpawning : MonoBehaviour
{

    private void OnEnable()
    {
        Collider collider = GetComponent<Collider>();
        if (Physics.OverlapSphere(transform.position, 6) is Collider[])
        {
            //PlaceItem
        }
        else
        {
            //try place at different location optimally
        }
        
        
    }
    
    

}
