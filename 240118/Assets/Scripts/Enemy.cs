using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Movement2D movement2D;         //이동관련 클래스
    [SerializeField] SpriteRenderer spriteRenderer; //그림렌더러
    [SerializeField] bool isLeft;                   //좌우 방향체크

    CapsuleCollider2D capsuleCollider;
    LayerMask groundMask;                           //바닥 레이어마스크

    bool isDead;                                    //사망 체크
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        groundMask = 1 << LayerMask.NameToLayer("Ground");//레이어마스크 할당

        isDead = false;
    }

    void Update()
    {
        RayCast();
        Movement(isDead);
    }

    /// <summary>
    /// 충돌 관련
    /// </summary>
    void RayCast()
    {
        float dir = isLeft ? -1.0f : 1.0f;//방향 체크
        /* 정면 체크 */
        Vector3 point = transform.position +
            (Vector3.right * 0.5f * dir) + (Vector3.up * capsuleCollider.size.y);//레이 시작지점 체크

        if(Physics2D.Raycast(point, Vector3.down, capsuleCollider.size.y, groundMask))
        {
            isLeft = !isLeft;
            dir = isLeft ? -1.0f : 1.0f;

            spriteRenderer.flipX = isLeft;
        }

        /* 절벽 체크 */
        point = transform.position + (Vector3.right * 0.3f * dir) + (Vector3.up * 0.5f);

        if(!Physics2D.Raycast(point, Vector3.down, 1.0f, groundMask))
        {
            isLeft = !isLeft;
            dir = isLeft ? -1.0f : 1.0f;

            spriteRenderer.flipX = isLeft;
        }
    }

    /// <summary>
    /// 이동 관련
    /// </summary>
    void Movement(bool dead)
    {
        if (!dead)
        {
            Vector2 moveDir = new Vector2(isLeft ? -1.0f : 1.0f, 0);//어느방향으로 갈지 체크
            movement2D.Movement(moveDir);
        }
        else
            movement2D.Movement(Vector2.zero);
    }

    /// <summary>
    /// 몬스터 피격 관련
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
            float dir = isLeft ? -1.0f : 1.0f;//방향 체크
            /* 정면 체크 레이 */
            Vector3 point = transform.position +
                (Vector3.right * 0.5f * dir) + (Vector3.up * capsuleCollider.size.y);//레이 시작지점 체크

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(point, Vector3.down * capsuleCollider.size.y);

            /* 절벽 체크 레이 */
            point = transform.position + (Vector3.right * 0.3f * dir) + (Vector3.up * 0.5f);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(point, Vector3.down * 1.0f);
        }
    }
}
