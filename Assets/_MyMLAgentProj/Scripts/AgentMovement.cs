using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private Transform firstTransform;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        rigid.MovePosition(transform.position + direction.normalized * Time.deltaTime * speed);
    }

    public void ResetMovement()
    {
        rigid.velocity = Vector3.zero;

        if (firstTransform)
        {
            transform.position = firstTransform.position;
            transform.rotation = firstTransform.rotation;
        }
    }
}
