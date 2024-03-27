using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Movement2D movement2D;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("플레이어Attack");

            collision.GetComponent<Enemy>().Hit();//Player레이어와 접촉시 활성화(Rayer세팅 참조)
            player.Movement.Throw(player.HitThrow);

            movement2D.JumpPlus(1);
        }
    }
}
