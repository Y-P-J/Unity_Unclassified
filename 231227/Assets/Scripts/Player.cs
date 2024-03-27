using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] SpriteAnimation anim;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityScale;
    [SerializeField] float radius;

    const float MIN_Y = -3.1f;
    const float MIN_ANGLE = -90f;
    const float MAX_ANGEL = 30f;

    float GRAVITY => -9.81f * gravityScale;         // 실제 중력 : 중력 상수 * 중력 배율
    float ANGLE_OFFSET => MAX_ANGEL - MIN_ANGLE;    // 최소 최대 각도의 차이

    Vector3 velocity;       // 현재 속력.
    float angle;            // 현재 회전 값.
    bool isActivate;        // 활성화 상태인가.
    bool isDead;            // 죽었는가.

    void Update()
    {
        if (isActivate)
        {
            Gravity();      // 중력 적용.
            Rotate();       // 회전 적용.
            Jump();         // 점프 키 입력.
            Collision();    // 충돌 체크.
        }
    }
    
    private void Gravity()
    {
        velocity.y += GRAVITY * Time.deltaTime;           // 중력 가속도를 프레임 단위로 더한다.
        transform.position += velocity * Time.deltaTime;  // 현재 속력 값을 통해 프레임 단위로 이동.
        if (transform.position.y <= MIN_Y)                // 높이가 최저보다 낮거나 같으면
        {
            // 최저 높이로 제한한다.
            Vector3 position = transform.position;
            position.y = MIN_Y;
            transform.position = position;    
        }
    }
    private void Rotate()
    {
        // velocity가 +면 위를 향하고 -면 아래로 향한다.
        float targetAngle = (velocity.y >= 0) ? MAX_ANGEL : MIN_ANGLE;
        float angleValue = ANGLE_OFFSET * Time.deltaTime * 3.5f;
        angle = Mathf.MoveTowards(angle, targetAngle, angleValue);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
    private void Jump(bool isForce = false)
    {
        // 마우스 왼쪽(0)을 눌렀을 경우
        // Height에 도달하기 위한 속력(=힘)

        // 죽어있지 않아야하고 마우스를 누르거나 Force일 경우.
        if ((Input.GetMouseButtonDown(0) || isForce) && !isDead)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
            angle = MAX_ANGEL;
        }
    }
    private void Collision()
    {
        // 내 위치 기준 radius반지름을 가진 원 영역 내부에 들어온 충돌체 감지.
        Collider2D target = Physics2D.OverlapCircle(transform.position, radius);

        if (target != null || transform.position.y <= MIN_Y)
        {
            PlayerDead();
        }
    }

    void PlayerDead()
    {
        isDead = true;
        anim.enabled = false;
        ScoreManager.instance.RecordScore();
        GameManager.Instance.OnGameOver();
    }

    public void OnActivate()
    {
        isActivate = true;
        Jump(true);
    }

    private void OnDrawGizmos()
    {
        // 에디터 상에서 특정 기호를 그려 디버깅할 수 있게 한다.
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
