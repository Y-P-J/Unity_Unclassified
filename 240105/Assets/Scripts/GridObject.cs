using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    [SerializeField] Transform[] lines;    // ������ �迭.

    private void Start()
    {
        // ������ ���� ����. (0�� ���, Ȧ��:�Ķ�, ¦��:����)
        for(int i = 0; i<lines.Length; i++)
        {
            SpriteRenderer sr = lines[i].GetComponent<SpriteRenderer>();
            if (i == 0)
                sr.color = Color.white;
            else
                sr.color = (i % 2 == 1) ? Color.blue : Color.red;
        }
    }

    public void Setup()
    {
        float height = MusicSheet.Instance.BeatHeight;  // 1���� ����.
        float offset = height / Setting.snapCount;      // snap���� ����.

        // ���� ī��Ʈ�� �ش��ϴ� ��ŭ�� Ȱ��ȭ�ϰ� �������� ��Ȱ��ȭ�Ѵ�.
        for(int i = 0; i<lines.Length; i++)
        {
            lines[i].gameObject.SetActive(i < Setting.snapCount);
            if (lines[i].gameObject.activeSelf)
                lines[i].localPosition = Vector3.up * offset * i;
        }
    }
}
