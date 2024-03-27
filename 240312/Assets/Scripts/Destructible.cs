using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] GameObject destructObject;

    public void Crush()
    {
        Instantiate(destructObject, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.rigidbody == null || _collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            return;

        float _force = _collision.rigidbody.velocity.magnitude * _collision.rigidbody.mass; //질량 * 속력

        Debug.Log(gameObject.transform.name + " : " +  _collision.rigidbody.velocity.magnitude * _collision.rigidbody.mass);

        health = Mathf.Clamp(health - _force, 0.0f, float.MaxValue);

        if (health <= 0.0f)
            Crush();
    }
}
