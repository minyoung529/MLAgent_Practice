using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPost : MonoBehaviour
{
    [SerializeField]
    private bool isRed;

    [SerializeField]
    private SoccerAgent myAgent;

    [SerializeField]
    private SoccerAgent oppositeAgent;

    [SerializeField]
    private Color color;

    private SoccerField soccerField;

    void Start()
    {
        soccerField = transform.GetComponentInParent<SoccerField>();
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("BALL"))
        {
            myAgent?.AddRewardToAgent(-5f);
            oppositeAgent?.AddRewardToAgent(5f);
            
            soccerField.AddScore(isRed);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
