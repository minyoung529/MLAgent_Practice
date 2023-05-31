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

    private void Update()
    {
        AddRewardToAgent(-Time.deltaTime * 0.25f);
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
        sensor.AddObservation(soccerField.BallPosition);
        sensor.AddObservation(transform.position);
        sensor.AddObservation((soccerField.BallPosition - transform.position).normalized);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float horizontal = actions.ContinuousActions[0];
        float vertical = actions.ContinuousActions[1];

        movement.Move(new Vector3(horizontal, 0f, vertical));

        if(actions.DiscreteActions[0] == 1) // shoot
        {
            shoot.Shoot();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        var cActions = actionsOut.ContinuousActions;
        cActions[0] = horizontal;
        cActions[1] = vertical;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var dActions = actionsOut.DiscreteActions;
            dActions[0] = 1;
        }
    }

    public void AddRewardToAgent(float value)
    {
        AddReward(value);
    }
}
