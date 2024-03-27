using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class TouchPlayer : MonoBehaviour
{
    NavMeshAgent agent;
    EntityMove move;
    Camera cam;
    LayerMask groundMask;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        move = GetComponent<EntityMove>();
        agent.speed = move.MoveSpeed;
        cam = Camera.main;
        groundMask = 1 << LayerMask.NameToLayer("Ground");
    }

    Vector3 point;
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, groundMask))
            {
                point = hit.point;

                agent.SetDestination(point);
            }
            else
                point = ray.origin;
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        Gizmos.DrawSphere(point, 0.5f);
    }
}
