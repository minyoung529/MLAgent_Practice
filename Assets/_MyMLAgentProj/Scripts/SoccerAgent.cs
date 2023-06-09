using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class SoccerAgent : Agent
{
    private AgentMovement movement;
    private SoccerField soccerField;
    private AgentShoot shoot;

    [SerializeField]
    private Transform oppositeGoalPost;

    private void Update()
    {
        Vector3 curpos = transform.localPosition;

        if (Mathf.Abs(curpos.x) > 20f || Mathf.Abs(curpos.z) > 33f)
        {
            AddRewardToAgent(-0.25f);
        }

        curpos.x = Mathf.Clamp(curpos.x, -20f, 20f);
        curpos.z = Mathf.Clamp(curpos.z, -33f, 33f);

        transform.localPosition = curpos;

        AddRewardToAgent(-Time.deltaTime * 0.26f);
    }

    public override void Initialize()
    {
        movement = GetComponent<AgentMovement>();
        shoot = GetComponent<AgentShoot>();
        soccerField = transform.GetComponentInParent<SoccerField>();
    }

    public override void OnEpisodeBegin()
    {
        movement?.ResetMovement();
        soccerField?.ResetField();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(soccerField.BallLocalPosition);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation((soccerField.BallLocalPosition - transform.localPosition).normalized);

        sensor.AddObservation(oppositeGoalPost.localPosition); // Opposite goal post position
        sensor.AddObservation((oppositeGoalPost.localPosition - soccerField.BallLocalPosition).normalized); // Opposite goal post position

        // sensor.AddObservation(Vector3.Distance(oppositeGoalPost.localPosition, soccerField.BallLocalPosition)); //

        // vec3 x 5 + float x 1
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float horizontal = actions.ContinuousActions[0];
        float vertical = actions.ContinuousActions[1];
        float rotation = actions.ContinuousActions[2];

        movement.Move(new Vector3(horizontal, 0f, vertical));

        transform.Rotate(Vector3.up * rotation * Time.deltaTime * 180f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        var cActions = actionsOut.ContinuousActions;
        cActions[0] = horizontal;
        cActions[1] = vertical;

        if (Input.GetKey(KeyCode.Q))
        {
            cActions[2] = -1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            cActions[2] = 1f;
        }
    }

    public void AddRewardToAgent(float value)
    {
        AddReward(value);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BALL"))
        {
            SoccerField.LastTouchPlayer = this;
            AddRewardToAgent(0.25f);
        }
    }
}
