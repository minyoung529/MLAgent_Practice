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

        curpos.x = Mathf.Clamp(curpos.x, -14f, 14f);
        curpos.z = Mathf.Clamp(curpos.z, -21f, 21f);

        transform.localPosition = curpos;

        AddRewardToAgent(Time.deltaTime * 0.26f);
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
        
        sensor.AddObservation(Vector3.Distance(oppositeGoalPost.localPosition, soccerField.BallLocalPosition)); //

        // vec3 x 5 + float x 1
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float horizontal = actions.ContinuousActions[0];
        float vertical = actions.ContinuousActions[1];

        movement.Move(new Vector3(horizontal, 0f, vertical));

        if (actions.DiscreteActions[0] == 1) // shoot
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

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    var dActions = actionsOut.DiscreteActions;
        //    dActions[0] = 1;
        //}
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
            AddRewardToAgent(1.5f);
        }
    }
}
