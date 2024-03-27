using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] //Rigidbody2D�� ����� �����Ѵ�
public class Movement2D : MonoBehaviour
{
    [SerializeField] float moveSpeed;           //�̵� �ӵ�
    [SerializeField] float jumpPower;           //������ ��
    [SerializeField] float groundRadius;        //Ground���̾� ���������� ������

    Rigidbody2D rigid;                          //������ٵ�2D
    bool isGrounded;                            //�ٴ� ���˿���
    int maxJumpCount;                           //�ִ� �������� Ƚ��
    int currentJumpCount;                       //���� �������� Ƚ��

    public bool IsGrounded => isGrounded;
    public float GroundRadius => groundRadius;
    public Vector2 Velocity => rigid.velocity;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();//�ڱ��ڽ� �Ҵ�
        isGrounded = false;//false�� ù �Ҵ�
        maxJumpCount = 2;//�ִ� ����Ƚ�� �Ҵ�
        currentJumpCount = maxJumpCount;//ù currentJumpCount�� maxJumpCount�� ������
    }

    void Update()
    {
        LayerMask groundMask = 1 << LayerMask.NameToLayer("Ground");
        isGrounded = Physics2D.OverlapCircle(transform.position, groundRadius, groundMask);//ground���̾ ������ �ݶ��̴��� �浹���� �� true�� ��ȯ�Ѵ�.

        //isGrounded�� true�Ͻ�(velocity�� ���װ������� ��ü�س���)
        if (isGrounded && rigid.velocity.y <= 0.01)
            JumpPlus(maxJumpCount);//���� Ƚ�� �ʱ�ȭ
    }

    /// <summary>
    /// �̵� ����
    /// </summary>
    public void Movement(Vector2 currentInput)
    {
        if (isGrounded == true && currentInput.y == -1)//���󿡼� �Ʒ�����Ű�� �Է����Ͻ�
            currentInput.x = 0.0f;//�¿��Է°��� ���ý�Ų��.

        rigid.velocity = new Vector2(moveSpeed * currentInput.x, rigid.velocity.y);//ĳ���� �¿� ��ǥ�̵�
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public bool Jump()
    {
        /* ���� ���ϴ� ���
         * ForceMode2D.Force : ���������� ���� ���Ѵ�
         * ForceMode2D.Impulse : �ѹ� �� �δ�
         */

        if (currentJumpCount <= 0)//��� ����Ƚ�� �Ҹ��
            return false;//false ��ȯ

        rigid.velocity = new Vector2(rigid.velocity.x, 0.0f);//y�� �ӷ��� �ʱ�ȭ�Ѵ�
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);//ĳ���Ϳ��� +y�� ���� ����

        currentJumpCount--;//�������� Ƚ���� 1 ���δ�

        return true;//true��ȯ
    }

    /// <summary>
    /// ����Ƚ�� ȸ��
    /// </summary>
    public void JumpPlus(int plus)
    {
        currentJumpCount =//maxJumpCount�� ���Ѽ����� ȸ����Ŵ
            currentJumpCount + plus > maxJumpCount ? maxJumpCount : currentJumpCount + plus;
    }

    /// <summary>
    /// �����̵� ����
    /// </summary>
    /// <param name="power"></param>
    /// <returns></returns>
    public bool Throw(float power)
    {
        rigid.velocity = new Vector2(rigid.velocity.x, 0.0f);//y�� �ӷ��� �ʱ�ȭ�Ѵ�
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);//���� jumpPower��ŭ�� ���� �ѹ� �ش�
        
        return true;
    }
}
