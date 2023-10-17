using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBeforeSpawning : MonoBehaviour
{

    private void OnEnable()
    {
        Collider collider = GetComponent<Collider>();
        if (Physics.OverlapSphere(transform.position, 6) == null)
        {
            //PlaceItem
        }
        else
        {
            //delete this object
        }
        
        
    }
    
    

}
