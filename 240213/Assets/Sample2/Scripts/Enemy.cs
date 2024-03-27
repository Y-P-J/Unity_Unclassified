using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

[RequireComponent(typeof(EntityMove))]
public class Enemy : MonoBehaviour
{
    [SerializeField] float detectRadius;        //탐지범위
    [SerializeField] float detectAngle;         //탐지각도
    [SerializeField] float patrolRadius;        //정찰범위
    [SerializeField] float patrolStayTime;      //정찰대기시간

    NavMeshAgent agent;
    EntityMove move;

    LayerMask playerMask;
    Transform target;

    Vector3 spawnPosition;                      //시작장소 저장
    Vector3 patrolPoint;                        //탐색포인트
    bool isChase;                               //추격여부

    void IsChaseChange(bool b) => isChase = b;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        move = GetComponent<EntityMove>();

        playerMask = LayerMask.GetMask("Player");
        spawnPosition = transform.position;
        patrolPoint = spawnPosition;
        isChase = false;
    }

    void Update()
    {
        if (Detect())
            Chase(target);
        else
            agent.SetDestination(Patrol());
    }

    Vector3 rayDirection;
    float angle;
    bool Detect()
    {
        //플레이어 탐지
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectRadius, playerMask);

        if (colliders.Length > 0)
        {
            //추격 상태일시 그 외 조건 무시하고 바로 true 반환
            if (isChase)
                return true;

            target = colliders[0].transform;
            rayDirection = target.position - transform.position;

            //탐지범위 판단
            Vector3 me = new Vector3(transform.position.x, 0.0f, transform.position.z);
            Vector3 you = new Vector3(target.position.x, 0.0f, target.position.z);
            angle = Vector3.Angle(transform.forward, you - me);

            if (angle > detectAngle * 0.5f)
                return false;

            //레이 발사 후 사이에 구조물이 있는지 확인
            Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, int.MaxValue);

            if (hit.collider.transform != target)
                return false;

            //모든 조건 확인 후 true 반환
            IsChaseChange(true);
            return true;
        }
        else//탐지범위 내 플레이어 없을시 추격종료
        {
            IsChaseChange(false);
            return false;
        }
    }

    float stayTime = 0.0f;
    Vector3 Patrol()
    {
        if (!agent.hasPath)
        {
            if (stayTime < patrolStayTime)
                stayTime += Time.deltaTime;
            else
            {
                stayTime = 0.0f;
                Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;

                patrolPoint = spawnPosition + new Vector3(randomCircle.x, 0.0f, randomCircle.y);
            }
        }

        return patrolPoint;
    }

    void Chase(Transform target)
    {
        transform.LookAt(target);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        agent.SetDestination(target.position);
    }

    private void OnDrawGizmos()
    {

        #region Detect 범위, 관련
        if (!isChase)
        {
            Gizmos.color = Color.red;
            UnityEditor.Handles.color = Color.red;

            UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, detectAngle * 0.5f, detectRadius);
            UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, transform.forward, -detectAngle * 0.5f, detectRadius);

            Quaternion rot = Quaternion.AngleAxis(detectAngle * 0.5f, Vector3.up);
            Vector3 posA = transform.position + rot * (transform.forward * detectRadius);
            Vector3 posB = transform.position + Vector3.Reflect(rot * (transform.forward * detectRadius), transform.right);
            Gizmos.DrawLine(transform.position, posA);
            Gizmos.DrawLine(transform.position, posB);

            Gizmos.color = Color.green;

            if (angle <= detectAngle * 0.5f)
                Gizmos.DrawRay(transform.position, rayDirection);
        }
        #endregion

        #region Patrol 범위, 관련
        if (!isChase)
        {
            Gizmos.color = Color.yellow;
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(spawnPosition, Vector3.up, patrolRadius);
            Gizmos.DrawSphere(patrolPoint, 0.5f);
        }
        #endregion

        #region Chase 범위, 관련
        if(isChase)
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, detectRadius);
        }
        #endregion
    }
}