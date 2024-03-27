using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Bullet
{
    [SerializeField] float explodeRange;
    [SerializeField] float explodePower;

    public override void Shoot(float moveSpeed, float damage)
    {
        base.Shoot(moveSpeed, damage);
        GetComponent<Rigidbody>().AddForce(transform.forward * moveSpeed, ForceMode.Impulse);
    }

    protected override void OnCollisionEnter(Collision collision)
    {

    }

    protected override void Update()
    {
        if ((showTime += Time.deltaTime) >= TimeToDestroyed)
            Explode();
    }

    private void Explode()
    {
        Instantiate(hitVfx, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRange);
        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnHit(HITTYPE.UPPER, damage);
                continue;
            }


            Destructible dest = collider.gameObject.GetComponent<Destructible>();
            if (dest != null)
                dest.Crush();
        }

        colliders = Physics.OverlapSphere(transform.position, explodeRange);

        foreach (Collider collider in colliders)
        {
            Rigidbody rigid = collider.gameObject.GetComponent<Rigidbody>();

            if(rigid != null)
            {
                rigid.AddExplosionForce(explodePower, transform.position, explodeRange);
            }
        }

        Release();
    }
}
