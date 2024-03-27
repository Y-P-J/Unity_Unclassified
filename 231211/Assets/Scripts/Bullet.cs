using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    const float GRAVITY = -9.81f * 3.4f;

    Vector3 origin;         //���� ���� ��ġ
    Vector3 velocity;       //�ӷ�

    void Start()
    {
        origin = transform.position;
    }
    
    public void AddForce(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    void Update()
    {
        velocity.y += GRAVITY * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;

        float carry = Vector3.Distance(transform.position, origin);
        if(carry >= 30)
        {
            Destroy(gameObject);
        }
    }
}
