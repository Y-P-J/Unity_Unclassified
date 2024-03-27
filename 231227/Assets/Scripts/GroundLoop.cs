using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLoop : MonoBehaviour
{
    [SerializeField] Transform[] grounds;   // ���� �׶��� �迭.
    [SerializeField] float speed;

    Vector3 minPosition;        // �ּ� ��ġ.

    void Start()
    {
        // x�� ��ġ ���� �������� �����Ѵ�.
        Array.Sort(grounds, (a1, a2) => {
            if (a1.position.x < a2.position.x)
                return -1;
            else if (a1.position.x > a2.position.x)
                return 1;
            else
                return 0;
        });

        // �ּ�,�ִ� ��ġ �� ����.
        minPosition = grounds[0].position;
    }

    void Update()
    {
        // ��� �׶��带 ���鼭...
        foreach(Transform ground in grounds)
        {
            // �׶��带 �������� speed��ŭ �ű�� ��ġ ���� min���� �������� max�� �̵��Ѵ�.
            ground.Translate(Vector3.left * speed * Time.deltaTime);
            if (ground.position.x <= minPosition.x)
                ground.position += Vector3.right * 6f * (grounds.Length);
        }
    }       
}
