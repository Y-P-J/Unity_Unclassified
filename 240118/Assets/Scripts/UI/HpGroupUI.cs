using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HpGroupUI : MonoBehaviour
{
    Player player;
    HpUI[] hps;

    public void Setup(Player player)
    {
        this.player = player;

        hps = GetComponentsInChildren<HpUI>(true);

        if (player.MaxHp > hps.Length)
        {
            int add = player.MaxHp - hps.Length;
            HpUI copy = hps[0];

            for (int i = 0; i < add; i++)
                Instantiate(copy, transform);

            hps = GetComponentsInChildren<HpUI>(true);
        }
        else
            for (int i = 0; i < hps.Length; i++)
                hps[i].gameObject.SetActive(i < player.MaxHp);
    }

    public void UpdateUI()
    {
        hps[player.curHp].SwitchToggle();
    }
}
