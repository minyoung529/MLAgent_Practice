using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentShoot : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField]
    private float radius = 3f;

    [SerializeField]
    private float force = 30f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Shoot()
    {
        rigid.AddExplosionForce(force, transform.position, radius);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
