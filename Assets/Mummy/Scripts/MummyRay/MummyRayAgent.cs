using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MummyRayAgent : Agent
{
    private MummyRayStage mummyRayStage;
    private int goodItemCount = 0;
    private float speed = 5f;

    public override void Initialize()
    {
        mummyRayStage = GetComponentInParent<MummyRayStage>();
    }

    public override void OnEpisodeBegin()
    {
        goodItemCount=0;
        mummyRayStage?.OnEpisodeBegin();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int forwardMove = actions.DiscreteActions[0];
        int rightMove = actions.DiscreteActions[1];

        if (forwardMove == 1) // Forward
        {
            transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
        }
        else if (forwardMove == 2) // Back
        {
            transform.Translate(-transform.forward * Time.deltaTime * speed, Space.World);
        }

        if (rightMove == 1) // Right
        {
            transform.Rotate(transform.up, Time.fixedDeltaTime * 200f);
        }
        else if (rightMove == 2) // Left
        {
            transform.Rotate(-transform.up, Time.fixedDeltaTime * 200f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actions[0] = 2;
        }

        if (Input.GetKey(KeyCode.D))
        {
            actions[1] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            actions[1] = 2;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("WALL"))
        {
            AddReward(-0.1f);
        }

        else if (other.gameObject.CompareTag("BAD_ITEM"))
        {
            AddReward(-1f);
            mummyRayStage.Fail(EndEpisode);
            mummyRayStage.DestroyItem(other.gameObject);
        }

        else if (other.gameObject.CompareTag("GOOD_ITEM"))
        {
            AddReward(1f);

            if (++goodItemCount >= 30)
            {
                mummyRayStage.Success(EndEpisode);
            }

            mummyRayStage.DestroyItem(other.gameObject);
        }
    }
}
