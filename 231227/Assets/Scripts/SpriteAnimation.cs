using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ش� ������Ʈ�� ����ϱ� ���� �����ϴ� ������Ʈ.
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    const float SWITCH_TIME = 0.5f;         // ��������Ʈ ��ȯ �ð�.

    [SerializeField] Sprite[] sprites;      // ��������Ʈ �迭.
    [SerializeField] float animSpeed;       // �ִϸ��̼� ���ǵ�.

    SpriteRenderer spriteRenderer;          // ��������Ʈ ������.

    float time = 0f;
    int index = 0;                          // ���� ������� ��������Ʈ�� ��ȣ.

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
