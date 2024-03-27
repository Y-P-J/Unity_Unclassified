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
        // ���� ������ �ð� ���� ������ ���� ���� �̵���Ų��.
        Vector3 start = back.localPosition + Vector3.left * back.localScale.x / 2f;
        float ratio = MusicSheet.Instance.audioSource.time / MusicSheet.Instance.audioSource.clip.length;
        circle.localPosition = start + Vector3.right * back.localScale.x * ratio;

        // TimeSpan : �ð� ���� ����ü.
        // = FromSeconds(��) : �ʸ� �޾Ƽ� '��:��:�и���'�� ��ȯ�Ѵ�.
        timeText.text = TimeSpan.FromSeconds(MusicSheet.Instance.audioSource.time).ToString(@"mm\:ss\:fff");
    }


}
