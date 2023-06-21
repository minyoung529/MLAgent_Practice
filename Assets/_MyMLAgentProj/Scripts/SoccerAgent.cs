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

        curpos.x = Mathf.Clamp(curpos.x, -19f, 19f);
        curpos.z = Mathf.Clamp(curpos.z, -32f, 32f);

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
        sensor.AddObservation(transform.position);
        sensor.AddObservation(oppositeGoalPost.position); // Opposite goal post position
        sensor.AddObservation(oppositeGoalPost.position - transform.position); // Opposite goal post position

        // vec3 x 5 + float x 1
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float horizontal = actions.ContinuousActions[0];
        float vertical = actions.ContinuousActions[1];

        movement.Move(new Vector3(horizontal, 0f, vertical));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        var cActions = actionsOut.ContinuousActions;
        cActions[0] = horizontal;
        cActions[1] = vertical;
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
            AddRewardToAgent(0.75f);
        }
    }
}
