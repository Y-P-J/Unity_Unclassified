using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("��Attack");

        collision.GetComponent<Player>().Hit();//Player���̾�� ���˽� Ȱ��ȭ(Rayer���� ����)
    }
}
