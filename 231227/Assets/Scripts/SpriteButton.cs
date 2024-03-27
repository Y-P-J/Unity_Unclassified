using System;
using UnityEngine;

// �̺�Ʈ ������ : ��������Ʈ, �������̽�

// Attribute(�Ӽ�) : Ư�� ���, ����, ���� ��.
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteButton : ObjectButton
{
    [SerializeField] Color normalColor;
    [SerializeField] Color hoverColor;
    
    SpriteRenderer spriteRenderer;

    // �ݶ��̴� : �浹ü + �浹 ����
    // Ʈ���� : �浹 ����
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
                
    }

    // ������Ʈ�� collider�� ���콺 �����Ͱ� ������ �� �Ҹ��� �̺�Ʈ �Լ�.
    private void OnMouseEnter()
    {
        spriteRenderer.color = hoverColor;
    }
    private void OnMouseExit()
    {
        spriteRenderer.color = normalColor;
    }

    private void OnMouseDown()
    {
        transform.localScale = Vector3.one * 0.9f;
    }
    private void OnMouseUp()
    {
        transform.localScale = Vector3.one;
    }

}
