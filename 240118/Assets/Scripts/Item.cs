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
        //만약 카운트를 0으로 적용해놨다면
        if (count <= 0)
        {
            count = 1;//1로 적용
        }

        GetComponent<Animator>().SetTrigger("on");//on트리거 활성화

        Player player = collision.GetComponent<Player>();//부딪힌 플레이어를 데리고와서

        player.GetItem(id, count);//아이템 지급
    }
}
