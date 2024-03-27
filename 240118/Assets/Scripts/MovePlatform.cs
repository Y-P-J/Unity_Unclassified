using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] Vector3 offset;
    [SerializeField] float moveSpeed;
    [SerializeField] float waitTime;

    List<Collider2D> colliders;
    Vector3 pointA;
    Vector3 pointB;
    Vector3 destination;
    bool isReverse;
    bool isMove;

    void Start()
    {
        colliders = new List<Collider2D>();

        pointA = transform.position;
        pointB = transform.position + offset;
        destination = isReverse ? pointB : pointA;

        isReverse = false;
        isMove = true;
    }

    void Update()
    {
        Move();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        colliders.Add(collision.collider);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        colliders.Remove(collision.collider);
    }

    void Move()
    {
        Vector3 prevPosition = transform.position;

        if (isMove)
            transform.position =
                Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        Vector3 movement = transform.position - prevPosition;
        foreach(Collider2D collider in colliders)
        {
            collider.transform.position += movement;
        }

        if (transform.position == destination)
        {
            isReverse = !isReverse;
            destination = isReverse ? pointB : pointA;

            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        isMove = false;

        yield return new WaitForSeconds(waitTime);

        isMove = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if(Application.isPlaying)
        {
            Gizmos.DrawLine(pointA, pointB);
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position + offset);
        }
    }
}
