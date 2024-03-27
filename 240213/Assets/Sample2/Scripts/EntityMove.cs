using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EntityMove : MonoBehaviour
{
    //[SerializeField] float gravityScale;        // �߷� ����.
    //[SerializeField] float jumpHeight;          // ���� ����.
    [SerializeField] float moveSpeed;            // �̵� �ӵ�.

    CharacterController controller;             // ��Ʈ�ѷ�.
    Vector3 velocity;                           // �ӷ�.
    //LayerMask groundMask;                       // ���� ����ũ.
    //bool isGrounded;                            // ���� �� �ִ°�?

    //float GRAVITY_VALUE => -9.81f * gravityScale;   // ���� �߷� ��.

    public float MoveSpeed => moveSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //groundMask = 1 << LayerMask.NameToLayer("Ground");
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        
    }

    public void Move(float x, float z)
    {
        if (x == 0 && z == 0)
            return;

        Vector3 dir = (Vector3.forward * z + Vector3.right * x).normalized;

        controller.Move(dir * moveSpeed * Time.deltaTime);
    }
}
