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

    float GRAVITY => -9.81f * gravityScale;         // ���� �߷� : �߷� ��� * �߷� ����
    float ANGLE_OFFSET => MAX_ANGEL - MIN_ANGLE;    // �ּ� �ִ� ������ ����

    Vector3 velocity;       // ���� �ӷ�.
    float angle;            // ���� ȸ�� ��.
    bool isActivate;        // Ȱ��ȭ �����ΰ�.
    bool isDead;            // �׾��°�.

    void Update()
    {
        if (isActivate)
        {
            Gravity();      // �߷� ����.
            Rotate();       // ȸ�� ����.
            Jump();         // ���� Ű �Է�.
            Collision();    // �浹 üũ.
        }
    }
    
    private void Gravity()
    {
        velocity.y += GRAVITY * Time.deltaTime;           // �߷� ���ӵ��� ������ ������ ���Ѵ�.
        transform.position += velocity * Time.deltaTime;  // ���� �ӷ� ���� ���� ������ ������ �̵�.
        if (transform.position.y <= MIN_Y)                // ���̰� �������� ���ų� ������
        {
            // ���� ���̷� �����Ѵ�.
            Vector3 position = transform.position;
            position.y = MIN_Y;
            transform.position = position;    
        }
    }
    private void Rotate()
    {
        // velocity�� +�� ���� ���ϰ� -�� �Ʒ��� ���Ѵ�.
        float targetAngle = (velocity.y >= 0) ? MAX_ANGEL : MIN_ANGLE;
        float angleValue = ANGLE_OFFSET * Time.deltaTime * 3.5f;
        angle = Mathf.MoveTowards(angle, targetAngle, angleValue);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
    private void Jump(bool isForce = false)
    {
        // ���콺 ����(0)�� ������ ���
        // Height�� �����ϱ� ���� �ӷ�(=��)

        // �׾����� �ʾƾ��ϰ� ���콺�� �����ų� Force�� ���.
        if ((Input.GetMouseButtonDown(0) || isForce) && !isDead)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
            angle = MAX_ANGEL;
        }
    }
    private void Collision()
    {
        // �� ��ġ ���� radius�������� ���� �� ���� ���ο� ���� �浹ü ����.
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
        // ������ �󿡼� Ư�� ��ȣ�� �׷� ������� �� �ְ� �Ѵ�.
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
