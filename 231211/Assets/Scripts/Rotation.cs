using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class Rotation : MonoBehaviour
{
    [SerializeField] LineRenderer target;
    [SerializeField] float range;
    void Start()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        /* 오일러각도
         * 사람이 이해하는 각도 체계
         */
        /* 쿼터니언(사원수)
         * 기계가 이해하는 각도 체계
         */
        /* Degree각도
         * 사람이 이해하는 각도 체계 0 ~ 360도
         */
        /* Radian각도
         * 기계가 이해하는 각도 체계, 호도법
         */
        // Deg2Ran = Degree to Radian = 각도 값을 체계에 맞게 변환시켜줌

        Vector3 targetPos = new Vector3 (5, 5);
        Vector3 myPos = new Vector3(1, 2);

        float x = targetPos.x - myPos.x;
        float y = targetPos.y - myPos.y;
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        float distance = Mathf.Sqrt(x * x + y * y);
        Debug.Log($"각도 : {angle}도");
        Debug.Log($"거리 : {distance}");

        x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        y = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

        Debug.Log($"내위치 : {myPos.x}, {myPos.y}");
        Debug.Log($"상대위치 : {x + myPos.x}, {y + myPos.y}");
    }

    void Update()
    {
        // 매 프레임마다 z축으로 1도씩 회전시킴
        //transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 1));

        // Vector3.forward = Vector(0, 0, 1)
        if (Input.GetKey(KeyCode.E))
            transform.rotation *= Quaternion.Euler(Vector3.forward * -60.0f * Time.deltaTime);
        if (Input.GetKey(KeyCode.Q))
            transform.rotation *= Quaternion.Euler(Vector3.forward * 60.0f * Time.deltaTime);

        target.positionCount = 360;

        for (int i = 0; i < 360; i++)
        {
            float radian = i * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(MathF.Cos(radian), MathF.Sin(radian));
            target.SetPosition(i, pos * range);
        }

        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90.0f);
        transform.position += dir.normalized * Time.deltaTime;
    }
}

/* 삼각함수
 * 직각 삼각형에서의 각 변에 대한 비율
 * 밑변을 a, 높이를 b, 빗변을 c라고 정의하고, 빗변과 밑변의 사이각을 A라고 가정하자.
 * 
 * 밑변 = a   |   높이 = b   |   빗변 = c   |   각도 = @
 * 
 * Sin(@) = c / b   |   Cos(@) = c / a   |   Tan(@) = b / a
 * 
 * c가 1일때, a = Cos(@)   |   b = Sin(@)
 * 
 * 반지름이 r일때
 * r^2(Cos^2@ + Sin^2@) = r^2
 * = Cos^2@ + Sin^2@ = 1
 */