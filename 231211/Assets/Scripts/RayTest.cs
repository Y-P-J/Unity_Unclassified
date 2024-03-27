using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnDrawGizmos()
    {        
        //특정 위치에서 특정 방향으로 광선을 발사해서 충돌 정보를 반환한다.
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.right, 10.0f);

        //기즈모를 빨간색으로 한다.
        Gizmos.color = Color.red;

        if (rayHit.collider != null)    //히트에 무언가 잡혔을때 잡힌 위치까지의 빨간 선을 긋는다.
        {
            Gizmos.DrawLine(transform.position, rayHit.point);
            Gizmos.DrawSphere(rayHit.point, 0.1f);      //충돌한 끝 지점에 점을 찍는다.
        }
        else                            //A와 B사이에 씬에서만 보이는 빨간 선을 긋는다.
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 10.0f);
        }
    }
}
