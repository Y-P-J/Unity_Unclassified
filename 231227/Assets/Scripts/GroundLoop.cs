using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLoop : MonoBehaviour
{
    [SerializeField] Transform[] grounds;   // 하위 그라운드 배열.
    [SerializeField] float speed;

    Vector3 minPosition;        // 최소 위치.

    void Start()
    {
        // x축 위치 값을 기준으로 정렬한다.
        Array.Sort(grounds, (a1, a2) => {
            if (a1.position.x < a2.position.x)
                return -1;
            else if (a1.position.x > a2.position.x)
                return 1;
            else
                return 0;
        });

        // 최소,최대 위치 값 저장.
        minPosition = grounds[0].position;
    }

    void Update()
    {
        // 모든 그라운드를 돌면서...
        foreach(Transform ground in grounds)
        {
            // 그라운드를 좌측으로 speed만큼 옮기고 위치 값이 min보다 작이지면 max로 이동한다.
            ground.Translate(Vector3.left * speed * Time.deltaTime);
            if (ground.position.x <= minPosition.x)
                ground.position += Vector3.right * 6f * (grounds.Length);
        }
    }       
}
