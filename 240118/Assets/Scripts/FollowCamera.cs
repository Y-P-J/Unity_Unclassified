using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;              //타겟의 위치
    [SerializeField] BoxCollider2D limitArea;       //카메라 영역

    [SerializeField] Vector3 offset;                //보정값
    [SerializeField] float cameraSpeed;             //카메라 이동속도

    Camera cam;                                     //자기 카메라 컴포넌트

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        float camWidth = cam.orthographicSize * cam.aspect;//카메라 가로크기
        float camHeight = cam.orthographicSize;//카메라 세로크기

        Vector2 limitPos = limitArea.transform.position;//카메라 영역 중심점
        float limitWidth = limitArea.size.x / 2.0f;//카메라 영역 가로 절반
        float limitHeight = limitArea.size.y / 2.0f;//카메라 영역 세로 절반

        Vector2 min =//카메라 최소값
            new Vector2(limitPos.x - limitWidth + camWidth, limitPos.y - limitHeight + camHeight);
        Vector2 max =//카메라 최대값
            new Vector2(limitPos.x + limitWidth - camWidth, limitPos.y + limitHeight - camHeight);

        Vector3 destination = target.position + offset;//도착점

        destination.x = Mathf.Clamp(destination.x, min.x, max.x);//도착점x 최소, 최대값
        destination.y = Mathf.Clamp(destination.y, min.y, max.y);//도착점y 최소, 최대값

        transform.position =//Lerp로 비율(cameraSpeed)만큼 도착점으로 따라감
            Vector3.Lerp(transform.position, destination, cameraSpeed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(limitArea.transform.position, limitArea.size);
    }
}
