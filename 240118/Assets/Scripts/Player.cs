using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Movement2D movement;       //이동관련 클래스

    [SerializeField] HpGroupUI hpGroupUI;       //체력UI 클래스
    [SerializeField] int maxHp;                 //최대 체력
    public int curHp { get; private set; }      //현재 체력

    float hitThrow;                             //피격시 날려지는 값
    Vector2 currentInput;                       //현재 입력값
    float godModeTime = 2.0f;                   //무적시간
    Dictionary<ITEM, int> itemCount;            //각 아이템별 갯수
    bool isGameClear;                           //클리어판정

    Animator anim;                              //애니메이터
    SpriteRenderer spriteRenderer;              //스프라이트 렌더러

    public int MaxHp => maxHp;
    public Movement2D Movement => movement;
    public float HitThrow => hitThrow;

    void Start()
    {
        curHp = maxHp;

        hpGroupUI.Setup(this);

        hitThrow = 10.0f;
        currentInput = Vector2.zero;//0 할당
        anim = GetComponent<Animator>();//자신의 Inspector에 있는 Animator 할당
        spriteRenderer = GetComponent<SpriteRenderer>();//자신의 Inspector에 있는 SpriteRenderer 할당
        itemCount = new Dictionary<ITEM, int>();//자신의 딕셔너리를 할당

        if (CheckPoint.checkPosition != null)
            transform.position = (Vector3)CheckPoint.checkPosition;
    }

    void Update()
    {
        if (isGameClear)//클리어판정이 활성화되면
            return;//update를 무효화한다.

        /// <summary>
        /// 플레이어 좌우이동 관련
        /// </summary>
        currentInput.x = Input.GetAxisRaw("Horizontal");//왼쪽방향키 -1, 미입력 0, 오른쪽방향키 1
        currentInput.y = Input.GetAxisRaw("Vertical");//아래쪽방향키 -1, 미입력 0, 위쪽방향키 1

        movement.Movement(currentInput);

        if (currentInput.x != 0)//미입력 상태가 아닐때
            spriteRenderer.flipX = currentInput.x < 0;//무엇이든 입력될때 적용된다.

        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("onJump");//onJump 애니메이션트리거 활성화
            movement.Jump();
        }//z키 입력시 Jump() 사용
    }

    void LateUpdate()
    {
        anim.SetBool("isRun", currentInput.x != 0.0f);//좌우 입력이 들어갔는지 체크
        anim.SetBool("isGround", movement.IsGrounded);//바닥에 접촉중인지 체크
        anim.SetBool("isCrouch", currentInput.y == -1.0f);//아래 입력중인지 체크
        anim.SetFloat("velocityY", Mathf.Round(movement.Velocity.y));//상하 이동속도 입력

        currentInput = Vector2.zero;//현재 좌우입력값을 초기화함
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Clear")//Clear태그와 충돌할시
        {
            movement.Movement(Vector2.zero);//이동값을 정지시키고
            isGameClear = true;//클리어판정을 활성화한다
        }
    }

    /// <summary>
    /// 피격 관련
    /// </summary>
    public void Hit()
    {
        if(movement.Throw(hitThrow))
        {
            curHp--;
            hpGroupUI.UpdateUI();

            anim.SetTrigger("onHurt");//애니메이션 onHurt 트리거 활성화
            StartCoroutine(IEGodMode());//갓모드를 코루틴 시킨다
        }
    }

    /// <summary>
    /// 플레이어 사망시
    /// </summary>
    void Dead()
    {

    }

    /// <summary>
    /// 아이템 획득
    /// </summary>
    public void GetItem(ITEM id, int count)
    {
        if (!itemCount.ContainsKey(id))//아이템 키값이 없다면
            itemCount.Add(id, 0);//해당 키값을 적용

        itemCount[id] += count;//카운트값만큼 해당 키값의 값을 추가

        Debug.Log($"{id} increase {count} / current = {itemCount[id]}");
    }

    /// <summary>
    /// 무적시간 및 행위
    /// </summary>
    /// <returns></returns>
    IEnumerator IEGodMode()
    {
        float godModeTime = this.godModeTime;//무적시간 받아오기
        float offset = godModeTime / 10.0f;//무적시간동안의 10번 간격으로 오프셋조정
        float time = offset;//흐르는 오프셋시간
        int prevLayer = gameObject.layer;//현재 레이어 저장

        gameObject.layer = LayerMask.NameToLayer("GodMode");//레이어 GodMode로 변경

        spriteRenderer.ChangeAlpha(0.9f);

        while((godModeTime -= Time.deltaTime) >= 0.0f)//무적시간동안 유지
        {
            if ((time -= Time.deltaTime) <= 0.0f)//오프셋시간 종료시
            {
                time = offset;//다시 조정하고

                spriteRenderer.enabled = !spriteRenderer.enabled;//렌더러 껏다키기
            }
            yield return null;//프레임 넘기기
        }

        spriteRenderer.enabled = true;//렌더러 켜기
        spriteRenderer.ChangeAlpha(1.0f);
        
        gameObject.layer = prevLayer;//이전 레이어로 다시 복구
    }

    /// <summary>
    /// editor용 그림판
    /// </summary>
    void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.red;//기즈모의 색상을 붉은색으로 변경
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, movement.GroundRadius);//카메라를 향해 바라보는 원모양의 기즈모를 그린다.
    }

    /* RigidBody에 관한 고려사항
     * 1. 질량 : 기본무게는 1이다.
     * 2. 저항 : 공기저항, 회전저항
     * 3. 마찰력 : 충돌체끼리
     */

    /* Physics2D.OverlapCircle(중심좌표, 반지름값, [레이어마스크])
         * 중심좌표를 기준으로 반지름값만큼 원을 그려 레이어마스크를 포함한 콜라이더에 대한 충돌을 판정한다.
         */

}

/// <summary>
/// 메소드 추가
/// </summary>
public static class Method
{
    /// <summary>
    /// SpriteRenderer관련 메소드추가, 알파값 변경
    /// </summary>
    public static void ChangeAlpha(this SpriteRenderer renderer, float alpha)
    {
        Color color = renderer.color;//렌더러값 구조체 저장
        color.a = alpha;//알파값변경
        renderer.color = color;//적용
    }
}