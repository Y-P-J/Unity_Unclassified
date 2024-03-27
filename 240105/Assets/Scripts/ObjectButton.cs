using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class ObjectButton : MonoBehaviour
{
    [SerializeField] UnityEvent onClick;    // Action과 같다.

    private void OnMouseUpAsButton()
    {
        // 누른 오브젝트 위에서 마우르를 해제했을 때 불리는 함수.
        // 객체?. : 해당 객체가 null이 아닐 경우 내부에 진입한다.
        onClick?.Invoke();
    }
}
