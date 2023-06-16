using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // 축구공에 넣기
    void OnCollisionStay2D(Collision2D other)
    {
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("WALL"))
        {
            if (Vector3.Distance(SoccerField.LastTouchPlayer.transform.position, transform.position) < 3.5f)
            {
                SoccerField.LastTouchPlayer.AddRewardToAgent(-0.25f);
                Debug.Log("금지");
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 3.5f);
    }
}
