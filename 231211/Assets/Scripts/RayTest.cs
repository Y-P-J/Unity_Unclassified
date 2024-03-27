using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTest : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {

    }

    private void OnDrawGizmos()
    {        
        //Ư�� ��ġ���� Ư�� �������� ������ �߻��ؼ� �浹 ������ ��ȯ�Ѵ�.
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector2.right, 10.0f);

        //����� ���������� �Ѵ�.
        Gizmos.color = Color.red;

        if (rayHit.collider != null)    //��Ʈ�� ���� �������� ���� ��ġ������ ���� ���� �ߴ´�.
        {
            Gizmos.DrawLine(transform.position, rayHit.point);
            Gizmos.DrawSphere(rayHit.point, 0.1f);      //�浹�� �� ������ ���� ��´�.
        }
        else                            //A�� B���̿� �������� ���̴� ���� ���� �ߴ´�.
        {
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * 10.0f);
        }
    }
}
