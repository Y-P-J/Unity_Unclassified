using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;

    Rigidbody2D rigid;
    CapsuleCollider2D capsule;

    bool isGrounded;

    void Start()
    {
        //자료형에 해당하는 객체를 검색해서 리턴한다.
        rigid = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        /* OverlapCircle : 특정 위치에 n의 반지름을 가진 원을 만들어 충돌체크
         * LayerMask : 특정 레이어를 가진 오브젝트만 충돌체크
         */
        isGrounded = Physics2D.OverlapCircle(transform.position, capsule.size.x * .1f, 1 << LayerMask.NameToLayer("Ground"));

        /* Input.GetAxis : -1.0f ~ 1.0f
         * Input.GetAxisRaw : -1, 0, 1
         */
        Vector2 velocity = rigid.velocity;
        velocity.x = Input.GetAxis("Horizontal") * moveSpeed;

        if(Input.GetKey(KeyCode.Space) && isGrounded)
        {
            /* ForceMode2D.Force : 지속적인 힘으로 민다.
             * ForceMode2D.Impulse : 한번에 힘을 가한다.
             */
            //rigid.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            //높이의 설정이 애매하기 때문에 아래의 공식을 사용한다.
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * -9.81f * rigid.gravityScale);
        }

        rigid.velocity = velocity;
    }

    private void OnDrawGizmos()
    {
        if (capsule != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, capsule.size.x * .1f);
        }
    }
}
