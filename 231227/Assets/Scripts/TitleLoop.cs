using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLoop : MonoBehaviour
{
    [SerializeField] float height;
    [SerializeField] float speed;

    Vector3 origin;     // 최초 위치.
    float angle;        // 각도 값.

    private void Start()
    {
        origin = transform.position;
    }

    void Update()
    {
        angle += 180 * speed * Time.deltaTime;
        float ratio = Mathf.Sin(angle * Mathf.Deg2Rad);

        transform.position = origin + Vector3.up * height * ratio;
    }
}
