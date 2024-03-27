using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EntityMove : MonoBehaviour
{
    //[SerializeField] float gravityScale;        // 중력 배율.
    //[SerializeField] float jumpHeight;          // 점프 높이.
    [SerializeField] float moveSpeed;            // 이동 속도.

    CharacterController controller;             // 컨트롤러.
    Vector3 velocity;                           // 속력.
    //LayerMask groundMask;                       // 지면 마스크.
    //bool isGrounded;                            // 땅에 서 있는가?

    //float GRAVITY_VALUE => -9.81f * gravityScale;   // 실제 중력 값.

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
