using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberUI : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] numbers;  // 렌더러.
    [SerializeField] Sprite[] sprites;          // 숫자 이미지.
    [SerializeField] float offset;              // 간격.
    [SerializeField] int num;                   // 숫자.

    void Update()
    {
        // 숫자를 문자열로 변경. 시작 위치 계산.
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
