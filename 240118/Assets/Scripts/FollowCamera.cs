using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;              //Ÿ���� ��ġ
    [SerializeField] BoxCollider2D limitArea;       //ī�޶� ����

    [SerializeField] Vector3 offset;                //������
    [SerializeField] float cameraSpeed;             //ī�޶� �̵��ӵ�

    Camera cam;                                     //�ڱ� ī�޶� ������Ʈ

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        float camWidth = cam.orthographicSize * cam.aspect;//ī�޶� ����ũ��
        float camHeight = cam.orthographicSize;//ī�޶� ����ũ��

        Vector2 limitPos = limitArea.transform.position;//ī�޶� ���� �߽���
        float limitWidth = limitArea.size.x / 2.0f;//ī�޶� ���� ���� ����
        float limitHeight = limitArea.size.y / 2.0f;//ī�޶� ���� ���� ����

        Vector2 min =//ī�޶� �ּҰ�
            new Vector2(limitPos.x - limitWidth + camWidth, limitPos.y - limitHeight + camHeight);
        Vector2 max =//ī�޶� �ִ밪
            new Vector2(limitPos.x + limitWidth - camWidth, limitPos.y + limitHeight - camHeight);

        Vector3 destination = target.position + offset;//������

        destination.x = Mathf.Clamp(destination.x, min.x, max.x);//������x �ּ�, �ִ밪
        destination.y = Mathf.Clamp(destination.y, min.y, max.y);//������y �ּ�, �ִ밪

        transform.position =//Lerp�� ����(cameraSpeed)��ŭ ���������� ����
            Vector3.Lerp(transform.position, destination, cameraSpeed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(limitArea.transform.position, limitArea.size);
    }
}
