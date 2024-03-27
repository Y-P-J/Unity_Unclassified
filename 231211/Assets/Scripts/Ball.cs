using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float angle;
    [SerializeField] float power;
    Rigidbody2D rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    //충돌체가 충돌한 경우 호출되는 이벤트함수
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //충돌한 물체가 플레이어일 경우 && 플레이어의 속력이 3보다 클 경우
        //(공이 플레이어를 밀치면서 강제로 속력이 생기기에 저점을 만들어놓는다)
        if (collision.gameObject.tag == "Player"
            && collision.otherRigidbody.velocity.magnitude > 3)
        {
            Vector3 dir = transform.position - collision.transform.position;
            dir.y = 0;
            dir.Normalize();

            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * power * dir.x;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * power;

            rigid.velocity = Vector2.zero;
            rigid.AddForce(new Vector2(x, y), ForceMode2D.Impulse);

        }
    }
}
