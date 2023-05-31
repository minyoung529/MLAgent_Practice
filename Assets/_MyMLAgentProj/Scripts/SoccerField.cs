using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerField : MonoBehaviour
{
    [SerializeField]
    private Transform ball;

    [SerializeField]
    private bool isTraining = false;

    #region Property
    public Vector3 BallPosition => ball.position;
    #endregion
    
    public void ResetField()
    {
        // ball Random Transform
    }
}
