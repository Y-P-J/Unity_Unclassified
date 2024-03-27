using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class Rotation : MonoBehaviour
{
    [SerializeField] LineRenderer target;
    [SerializeField] float range;
    void Start()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        /* ���Ϸ�����
         * ����� �����ϴ� ���� ü��
         */
        /* ���ʹϾ�(�����)
         * ��谡 �����ϴ� ���� ü��
         */
        /* Degree����
         * ����� �����ϴ� ���� ü�� 0 ~ 360��
         */
        /* Radian����
         * ��谡 �����ϴ� ���� ü��, ȣ����
         */
        // Deg2Ran = Degree to Radian = ���� ���� ü�迡 �°� ��ȯ������

        Vector3 targetPos = new Vector3 (5, 5);
        Vector3 myPos = new Vector3(1, 2);

        float x = targetPos.x - myPos.x;
        float y = targetPos.y - myPos.y;
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        float distance = Mathf.Sqrt(x * x + y * y);
        Debug.Log($"���� : {angle}��");
        Debug.Log($"�Ÿ� : {distance}");

        x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        y = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

        Debug.Log($"����ġ : {myPos.x}, {myPos.y}");
        Debug.Log($"�����ġ : {x + myPos.x}, {y + myPos.y}");
    }

    void Update()
    {
        // �� �����Ӹ��� z������ 1���� ȸ����Ŵ
        //transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 1));

        // Vector3.forward = Vector(0, 0, 1)
        if (Input.GetKey(KeyCode.E))
            transform.rotation *= Quaternion.Euler(Vector3.forward * -60.0f * Time.deltaTime);
        if (Input.GetKey(KeyCode.Q))
            transform.rotation *= Quaternion.Euler(Vector3.forward * 60.0f * Time.deltaTime);

        target.positionCount = 360;

        for (int i = 0; i < 360; i++)
        {
            float radian = i * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(MathF.Cos(radian), MathF.Sin(radian));
            target.SetPosition(i, pos * range);
        }

        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90.0f);
        transform.position += dir.normalized * Time.deltaTime;
    }
}

/* �ﰢ�Լ�
 * ���� �ﰢ�������� �� ���� ���� ����
 * �غ��� a, ���̸� b, ������ c��� �����ϰ�, ������ �غ��� ���̰��� A��� ��������.
 * 
 * �غ� = a   |   ���� = b   |   ���� = c   |   ���� = @
 * 
 * Sin(@) = c / b   |   Cos(@) = c / a   |   Tan(@) = b / a
 * 
 * c�� 1�϶�, a = Cos(@)   |   b = Sin(@)
 * 
 * �������� r�϶�
 * r^2(Cos^2@ + Sin^2@) = r^2
 * = Cos^2@ + Sin^2@ = 1
 */