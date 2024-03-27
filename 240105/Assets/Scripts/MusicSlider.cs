using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MusicSlider : MonoBehaviour
{
    [SerializeField] Transform back;
    [SerializeField] Transform circle;
    [SerializeField] TMP_Text timeText;

    private void Update()
    {
        // 현재 음악의 시간 값의 비율에 따라 원을 이동시킨다.
        Vector3 start = back.localPosition + Vector3.left * back.localScale.x / 2f;
        float ratio = MusicSheet.Instance.audioSource.time / MusicSheet.Instance.audioSource.clip.length;
        circle.localPosition = start + Vector3.right * back.localScale.x * ratio;

        // TimeSpan : 시간 관련 구조체.
        // = FromSeconds(초) : 초를 받아서 '분:초:밀리초'로 변환한다.
        timeText.text = TimeSpan.FromSeconds(MusicSheet.Instance.audioSource.time).ToString(@"mm\:ss\:fff");
    }


}
