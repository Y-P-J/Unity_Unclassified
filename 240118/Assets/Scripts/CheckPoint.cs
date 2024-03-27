using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public static Vector3? checkPosition;//초기값을 null로 설정

    void OnTriggerEnter2D(Collider2D collision)
    {
        checkPosition = collision.transform.position;
        enabled = false;
    }
}
