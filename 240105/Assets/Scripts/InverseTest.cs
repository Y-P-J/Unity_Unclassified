using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseTest : MonoBehaviour
{
    [SerializeField] Transform pivot1;
    [SerializeField] Transform pivot2;

    private void Start()
    {
        Vector3 world = new Vector3(3, 4);
        Vector3 pivot1Local = pivot1.InverseTransformPoint(world);
        Vector3 pivot2Local = pivot2.InverseTransformPoint(world);
        
        Debug.Log($"world:{world}");
        Debug.Log($"local1:{pivot1Local}");
        Debug.Log($"local2:{pivot2Local}");
    }
}
