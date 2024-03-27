using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VectorSpace : MonoBehaviour
{
    [SerializeField] Transform target;
    //[SerializeField] float speed = 3.0f;

    void Start()
    {
        #region 절대좌표, 상대좌표
        /* 월드(절대)좌표 | transform.position
         * 좌표의 위치가 변환되지 않는 절대적인 좌표값
         * 
         * 로컬(상대)좌표 | transform.localPosition
         * 특정 위치를 기준으로 가지는 좌표값
         * 
         */

        /* 우측 벡터 기준
         * Vector3.right
         * 절대좌표 기준으로 우측을 향하는 단위벡터
         * 
         * transform.right
         * 상대좌표 기준으로 우측을 향하는 단위벡터
         */

        //transform.position = new Vector3(3, 3);           //절대좌표 기준으로 3, 3으로 옮긴다.
        //transform.localPosition = new Vector3(3, 3);      //상대좌표 기준으로 3, 3으로 옮긴다.

        //transform.position += Vector3.right * 3;          //절대좌표 기준으로 3만큼 이동해라
        //transform.position += transform.right * 3;        //상대좌표 기준으로 3만큼 이동해라

        //transform.Translate(Vector3.right);               //상대좌표 기준 이동량만큼 움직여라.
        //transform.Translate(Vector3.right, Space.World);  //절대좌표 기준 이동량만큼 움직여라.
        //transform.Translate(Vector3.right, target);       //해당 오브젝트의 상대좌표 기준 이동량만큼 움직여라.

        //Debug.Log(transform.root.name);                   //계층구조상 최상위 객체의 이름을 반환한다.
        //Debug.Log(transform.parent.name);                 //계층구조상 부모 객체의 이름을 반환한다. 
        #endregion

        #region 절대/상대좌표 회전
        /* 절대좌표 기준 회전축
         * transform.rotation
         * 
         * 상대좌표 시준 회전축
         * transform.localRotation
         */

        //transform.rotation = Quaternion.Euler(0, 0, 30);              //절대좌표 기준으로 30도 회전한다.
        //transform.localRotation = Quaternion.Euler(0, 0, 30);         //상대좌표 기준으로 30도 회전한다.

        //transform.Rotate(Vector3.forward * 30f);                      //상대좌표 기준으로 30도 회전한다.

        /* ※※※ 묶기위해 start로 옮겼으나 update에서 사용시 계속 회전함※※※ */
        //transform.RotateAround(target.position, Vector3.forward, 1f); //해당 오브젝트를 축으로 회전한다. 
        #endregion
    }

    void Update()
    {
        #region 타겟을 향한 이동 및 세부 조정
        /* Vector3.Distance
         * A와 B사이의 거리값을 반환한다.
         * 
         * A와 B의 거리를 계산해 이동거리보다 가깝다면 그 거리만큼만 이동한다.
         * 그외엔 기본적인 거리만큼만 움직인다.
         */

        //float distance = Vector3.Distance(transform.position, target.position);
        //float movement = speed * Time.deltaTime;

        //Vector3 dir = (target.position - transform.position).normalized;

        //if (distance < movement)
        //    transform.position += dir * distance;
        //else
        //    transform.position += dir * movement;



        /* Vector3.MoveTowards
         * A가 B위치로 C만큼 움직였을때의 위치값을 반환하며, 등속운동하여 일정한 속도로 이동한다(위 코드와 똑같음)
         * 
         * Vector3.Lerp
         * 거리에 따라 속도에 변화가 생겨 부드럽게 움직인다
         */

        //transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime); 
        //transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        #endregion
    }
}
