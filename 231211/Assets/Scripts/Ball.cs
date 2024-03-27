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

    //�浹ü�� �浹�� ��� ȣ��Ǵ� �̺�Ʈ�Լ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�浹�� ��ü�� �÷��̾��� ��� && �÷��̾��� �ӷ��� 3���� Ŭ ���
        //(���� �÷��̾ ��ġ�鼭 ������ �ӷ��� ����⿡ ������ �������´�)
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
