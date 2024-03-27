using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("적Attack");

        collision.GetComponent<Player>().Hit();//Player레이어와 접촉시 활성화(Rayer세팅 참조)
    }
}
