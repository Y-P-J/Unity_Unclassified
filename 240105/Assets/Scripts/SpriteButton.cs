using UnityEngine;

// 이벤트 제어자 : 델리게이트, 인터페이스

// Attribute(속성) : 특정 기능, 반응, 내장 값.
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteButton : ObjectButton
{
    [SerializeField] Color normalColor;
    [SerializeField] Color hoverColor;
    
    SpriteRenderer spriteRenderer;

    // 콜라이더 : 충돌체 + 충돌 영역
    // 트리거 : 충돌 영역
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 오브젝트의 collider에 마우스 포인터가 들어왔을 때 불리는 이벤트 함수.
    private void OnMouseEnter()
    {
        spriteRenderer.color = hoverColor;
    }
    private void OnMouseExit()
    {
        spriteRenderer.color = normalColor;
    }

    private Vector3 size;
    private void OnMouseDown()
    {
        size = transform.localScale;
        transform.localScale = size * 0.9f;
    }
    private void OnMouseUp()
    {
        transform.localScale = size;
    }

}
