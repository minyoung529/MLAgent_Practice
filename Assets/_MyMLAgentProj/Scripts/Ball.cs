using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // 축구공에 넣기
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("WALL"))
        {
            SoccerField.LastTouchPlayer.AddRewardToAgent(-1f);
        }
    }
}
