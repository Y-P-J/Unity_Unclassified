using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class ObjectButton : MonoBehaviour
{
    [SerializeField] UnityEvent onClick;    // Action�� ����.

    private void OnMouseUpAsButton()
    {
        // ���� ������Ʈ ������ ���츣�� �������� �� �Ҹ��� �Լ�.
        // ��ü?. : �ش� ��ü�� null�� �ƴ� ��� ���ο� �����Ѵ�.
        onClick?.Invoke();
    }
}
