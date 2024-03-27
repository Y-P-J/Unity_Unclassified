using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] //Rigidbody2D의 사용을 강제한다
public class Movement2D : MonoBehaviour
{
    [SerializeField] float moveSpeed;           //이동 속도
    [SerializeField] float jumpPower;           //점프의 힘
    [SerializeField] float groundRadius;        //Ground레이어 판정에대한 반지름

    Rigidbody2D rigid;                          //리지드바디2D
    bool isGrounded;                            //바닥 접촉여부
    int maxJumpCount;                           //최대 점프가능 횟수
    int currentJumpCount;                       //현재 점프가능 횟수

    public bool IsGrounded => isGrounded;
    public float GroundRadius => groundRadius;
    public Vector2 Velocity => rigid.velocity;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();//자기자신 할당
        isGrounded = false;//false로 첫 할당
        maxJumpCount = 2;//최대 점프횟수 할당
        currentJumpCount = maxJumpCount;//첫 currentJumpCount를 maxJumpCount로 설정함
    }

    void Update()
    {
        LayerMask groundMask = 1 << LayerMask.NameToLayer("Ground");
        isGrounded = Physics2D.OverlapCircle(transform.position, groundRadius, groundMask);//ground레이어를 포함한 콜라이더에 충돌했을 시 true를 반환한다.

        //isGrounded가 true일시(velocity는 버그관련으로 대체해놓음)
        if (isGrounded && rigid.velocity.y <= 0.01)
            JumpPlus(maxJumpCount);//점프 횟수 초기화
    }

    /// <summary>
    /// 이동 관련
    /// </summary>
    public void Movement(Vector2 currentInput)
    {
        if (isGrounded == true && currentInput.y == -1)//지상에서 아래방향키를 입력중일시
            currentInput.x = 0.0f;//좌우입력값을 무시시킨다.

        rigid.velocity = new Vector2(moveSpeed * currentInput.x, rigid.velocity.y);//캐릭터 좌우 좌표이동
    }

    /// <summary>
    /// 점프 관련
    /// </summary>
    public bool Jump()
    {
        /* 힘을 가하는 방식
         * ForceMode2D.Force : 지속적으로 힘을 가한다
         * ForceMode2D.Impulse : 한번 딱 민다
         */

        if (currentJumpCount <= 0)//모든 점프횟수 소모시
            return false;//false 반환

        rigid.velocity = new Vector2(rigid.velocity.x, 0.0f);//y의 속력을 초기화한다
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);//캐릭터에게 +y의 힘을 가함

        currentJumpCount--;//점프가능 횟수를 1 줄인다

        return true;//true반환
    }

    /// <summary>
    /// 점프횟수 회복
    /// </summary>
    public void JumpPlus(int plus)
    {
        currentJumpCount =//maxJumpCount를 상한선으로 회복시킴
            currentJumpCount + plus > maxJumpCount ? maxJumpCount : currentJumpCount + plus;
    }

    /// <summary>
    /// 강제이동 관련
    /// </summary>
    /// <param name="power"></param>
    /// <returns></returns>
    public bool Throw(float power)
    {
        rigid.velocity = new Vector2(rigid.velocity.x, 0.0f);//y의 속력을 초기화한다
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);//위로 jumpPower만큼의 힘을 한번 준다
        
        return true;
    }
}
