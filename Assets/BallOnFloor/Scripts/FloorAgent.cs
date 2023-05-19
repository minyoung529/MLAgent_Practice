using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class FloorAgent : Agent
{
    [SerializeField]
    private GameObject ball;
    private Rigidbody ballRigid;

    public override void Initialize()
    {
        ballRigid = ball.GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        transform.rotation = Quaternion.identity;

        // Random Value Setting
        transform.Rotate(Vector3.right, Random.Range(-10f,10f));    // Random Floor Rotation
        transform.Rotate(Vector3.forward, Random.Range(-10f,10f));

        ballRigid.velocity = Vector3.zero;
        ball.transform.localPosition = new Vector3(Random.Range(-1.5f, 1.5f), 1.5f, Random.Range(-1.5f, 1.5f)); // Random Ball Position
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Vector Observation
        sensor.AddObservation(transform.rotation.z);    // 1
        sensor.AddObservation(transform.rotation.x);    // 1
        sensor.AddObservation(ball.transform.position - transform.position);    // 3
        sensor.AddObservation(ballRigid.velocity);  // 3

        // => 8
    }

    // continuous actions => x and z
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Rotate Continuous 
        float zRotation = Mathf.Clamp(actions.ContinuousActions[0], -1, 1);
        float xRotation = Mathf.Clamp(actions.ContinuousActions[1], -1, 1);

        transform.Rotate(Vector3.forward, zRotation);   // z
        transform.Rotate(Vector3.right, xRotation);     // x

        if(DropBall())
        {
            SetReward(-1f);     // Bad Reward 
            EndEpisode();       // End Episode
        }
        else
        {
            SetReward(0.1f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continousActionsOut = actionsOut.ContinuousActions;
        continousActionsOut[0] = -Input.GetAxis("Horizontal");
        continousActionsOut[1] = Input.GetAxis("Vertical");
    }

    private bool DropBall()
    {
        bool yDiff = ball.transform.position.y - transform.position.y < -2f;
        bool outOfRangeX = Mathf.Abs(ball.transform.position.x - transform.position.x) > 2.5f; 
        bool outOfRangeZ = Mathf.Abs(ball.transform.position.z - transform.position.z) > 2.5f; 

        return yDiff || outOfRangeX || outOfRangeZ;
    }
}
