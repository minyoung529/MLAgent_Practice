using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private Transform firstPosition;

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

        if (firstPosition)
        {
            transform.position = firstPosition.position;
        }
    }
}
