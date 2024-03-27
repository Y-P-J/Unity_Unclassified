using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Movement2D movement2D;         //�̵����� Ŭ����
    [SerializeField] SpriteRenderer spriteRenderer; //�׸�������
    [SerializeField] bool isLeft;                   //�¿� ����üũ

    CapsuleCollider2D capsuleCollider;
    LayerMask groundMask;                           //�ٴ� ���̾��ũ

    bool isDead;                                    //��� üũ
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        groundMask = 1 << LayerMask.NameToLayer("Ground");//���̾��ũ �Ҵ�

        isDead = false;
    }

    void Update()
    {
        RayCast();
        Movement(isDead);
    }

    /// <summary>
    /// �浹 ����
    /// </summary>
    void RayCast()
    {
        float dir = isLeft ? -1.0f : 1.0f;//���� üũ
        /* ���� üũ */
        Vector3 point = transform.position +
            (Vector3.right * 0.5f * dir) + (Vector3.up * capsuleCollider.size.y);//���� �������� üũ

        if(Physics2D.Raycast(point, Vector3.down, capsuleCollider.size.y, groundMask))
        {
            isLeft = !isLeft;
            dir = isLeft ? -1.0f : 1.0f;

            spriteRenderer.flipX = isLeft;
        }

        /* ���� üũ */
        point = transform.position + (Vector3.right * 0.3f * dir) + (Vector3.up * 0.5f);

        if(!Physics2D.Raycast(point, Vector3.down, 1.0f, groundMask))
        {
            isLeft = !isLeft;
            dir = isLeft ? -1.0f : 1.0f;

            spriteRenderer.flipX = isLeft;
        }
    }

    /// <summary>
    /// �̵� ����
    /// </summary>
    void Movement(bool dead)
    {
        if (!dead)
        {
            Vector2 moveDir = new Vector2(isLeft ? -1.0f : 1.0f, 0);//����������� ���� üũ
            movement2D.Movement(moveDir);
        }
        else
            movement2D.Movement(Vector2.zero);
    }

    /// <summary>
    /// ���� �ǰ� ����
    /// </summary>
    public void Hit()
    {
        GetComponent<Animator>().SetTrigger("onDead");

        isDead = true;
    }

    void OnDrawGizmosSelected()
    {
        if(Application.isPlaying)
        {
            float dir = isLeft ? -1.0f : 1.0f;//���� üũ
            /* ���� üũ ���� */
            Vector3 point = transform.position +
                (Vector3.right * 0.5f * dir) + (Vector3.up * capsuleCollider.size.y);//���� �������� üũ

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(point, Vector3.down * capsuleCollider.size.y);

            /* ���� üũ ���� */
            point = transform.position + (Vector3.right * 0.3f * dir) + (Vector3.up * 0.5f);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(point, Vector3.down * 1.0f);
        }
    }
}
