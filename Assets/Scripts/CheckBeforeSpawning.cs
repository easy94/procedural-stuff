using UnityEngine;

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
