using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    [SerializeField] Transform[] lines;    // 스냅선 배열.

    private void Start()
    {
        // 라인의 색상 세팅. (0은 흰색, 홀수:파랑, 짝수:빨강)
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
        float height = MusicSheet.Instance.BeatHeight;  // 1박자 높이.
        float offset = height / Setting.snapCount;      // snap선의 간격.

        // 스냅 카운트에 해당하는 만큼만 활성화하고 나머지는 비활성화한다.
        for(int i = 0; i<lines.Length; i++)
        {
            lines[i].gameObject.SetActive(i < Setting.snapCount);
            if (lines[i].gameObject.activeSelf)
                lines[i].localPosition = Vector3.up * offset * i;
        }
    }
}
