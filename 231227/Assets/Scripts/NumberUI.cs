using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberUI : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] numbers;  // ������.
    [SerializeField] Sprite[] sprites;          // ���� �̹���.
    [SerializeField] float offset;              // ����.
    [SerializeField] int num;                   // ����.

    void Update()
    {
        // ���ڸ� ���ڿ��� ����. ���� ��ġ ���.
        string str = num.ToString();
        float startX = -offset * (str.Length - 1) / 2f;

        for (int i = 0; i<numbers.Length; i++)
        {
            bool isActivate = i < str.Length;
            numbers[i].gameObject.SetActive(isActivate);
            if (isActivate)
            {
                numbers[i].sprite = sprites[str[i] - '0'];
                numbers[i].transform.localPosition = Vector3.right * (startX + offset * i);
            }
        }
    }

    public void SetNumber(int num)
    {
        this.num = num;
    }

}
