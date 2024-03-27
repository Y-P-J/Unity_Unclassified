using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM
{
    GEM,
    CHERRY
}

public class Item : MonoBehaviour
{
    [SerializeField] ITEM id;
    [SerializeField] int count;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //���� ī��Ʈ�� 0���� �����س��ٸ�
        if (count <= 0)
        {
            count = 1;//1�� ����
        }

        GetComponent<Animator>().SetTrigger("on");//onƮ���� Ȱ��ȭ

        Player player = collision.GetComponent<Player>();//�ε��� �÷��̾ ������ͼ�

        player.GetItem(id, count);//������ ����
    }
}
