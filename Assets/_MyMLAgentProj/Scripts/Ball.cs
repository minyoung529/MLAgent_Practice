using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 originalPos;
    private Rigidbody rigid;

    [SerializeField]
    private float dist = 6f;
    
    void Awake()
    {
        originalPos = transform.position;
        rigid = GetComponent<Rigidbody>();
    }

    public void ResetPosition()
    {
        transform.position = originalPos;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    // 축구공에 넣기
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("WALL"))
        {
            if (Vector3.Distance(SoccerField.LastTouchPlayer.transform.position, transform.position) < dist)
            {
                SoccerField.LastTouchPlayer.AddRewardToAgent(-0.1f);
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dist);
    }
}
