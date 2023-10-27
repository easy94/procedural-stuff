using UnityEngine;

[ExecuteInEditMode]
public class CheckBeforeSpawning : MonoBehaviour
{
    private void Start()
    {
        if (Physics.OverlapSphere(transform.position, 4, ~1) is Collider[] s)
        {
            if(s.Length > 1)
            {
                DestroyImmediate(gameObject);
            }
        }
        
    }
    
}
