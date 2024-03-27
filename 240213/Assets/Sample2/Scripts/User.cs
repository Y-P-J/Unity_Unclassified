using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityMove))]
public class User : MonoBehaviour
{
    EntityMove move;

    void Start()
    {
        move = GetComponent<EntityMove>();
    }
    void Update()
    {
        Move();
    }

    void Move()
    {
        move.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
