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
        //�ڷ����� �ش��ϴ� ��ü�� �˻��ؼ� �����Ѵ�.
        rigid = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        /* OverlapCircle : Ư�� ��ġ�� n�� �������� ���� ���� ����� �浹üũ
         * LayerMask : Ư�� ���̾ ���� ������Ʈ�� �浹üũ
         */
        isGrounded = Physics2D.OverlapCircle(transform.position, capsule.size.x * .1f, 1 << LayerMask.NameToLayer("Ground"));

        /* Input.GetAxis : -1.0f ~ 1.0f
         * Input.GetAxisRaw : -1, 0, 1
         */
        Vector2 velocity = rigid.velocity;
        velocity.x = Input.GetAxis("Horizontal") * moveSpeed;

        if(Input.GetKey(KeyCode.Space) && isGrounded)
        {
            /* ForceMode2D.Force : �������� ������ �δ�.
             * ForceMode2D.Impulse : �ѹ��� ���� ���Ѵ�.
             */
            //rigid.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            //������ ������ �ָ��ϱ� ������ �Ʒ��� ������ ����Ѵ�.
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
