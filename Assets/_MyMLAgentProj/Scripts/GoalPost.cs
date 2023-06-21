using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPost : MonoBehaviour
{
    [SerializeField]
    private bool isRed;

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
            soccerField.AddScore(isRed);

            other.gameObject.GetComponent<Ball>().ResetPosition();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
