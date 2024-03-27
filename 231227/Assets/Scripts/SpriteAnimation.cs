using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 컴포넌트를 사용하기 위해 강제하는 컴포넌트.
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    const float SWITCH_TIME = 0.5f;         // 스프라이트 전환 시간.

    [SerializeField] Sprite[] sprites;      // 스프라이트 배열.
    [SerializeField] float animSpeed;       // 애니메이션 스피드.

    SpriteRenderer spriteRenderer;          // 스프라이트 렌더러.

    float time = 0f;
    int index = 0;                          // 현재 출력중인 스프라이트의 번호.

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        time += Time.deltaTime * animSpeed;
        if(time >= SWITCH_TIME)
        {
            time -= SWITCH_TIME;
            index += 1;
            if (index >= sprites.Length)
                index = 0;

            spriteRenderer.sprite = sprites[index];
        }
    }
}
