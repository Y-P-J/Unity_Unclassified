using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();//�ش� ������Ʈ�� ���ͼ�
        player.Hit();//Hit��Ŵ
    }
}
