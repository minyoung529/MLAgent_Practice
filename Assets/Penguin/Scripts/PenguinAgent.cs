using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PenguinAgent : Agent
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;
    public GameObject heartPrefab;
    public GameObject regurgitatedFisnPrefab;

    private PenguinArea penguinArea;
    new private Rigidbody rigidbody;
    private GameObject baby;

    private bool isFull;

    public override void Initialize()
    {
        penguinArea = transform.parent.GetComponentInChildren<PenguinArea>();
        rigidbody = GetComponent<Rigidbody>();
        baby = penguinArea.penguinBaby;
    }

    public override void OnEpisodeBegin()
    {
        isFull = false;
        penguinArea.ResetArea();

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(isFull); // 1
        sensor.AddObservation(Vector3.Distance(baby.transform.position, transform.position));   // 1
        sensor.AddObservation((baby.transform.position - transform.position).normalized);       // 3
        sensor.AddObservation(transform.forward);   // 3

        // => 8
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // 0 move or not?
        // 1 => left turn
        // 2 => right turn

        float forwardAmount = actions.DiscreteActions[0];
        float turnAmount = 0f;

        if (actions.DiscreteActions[1] == 1f)
        {
            turnAmount = -1f;
        }
        else if (actions.DiscreteActions[1] == 2f)
        {
            turnAmount = 1f;
        }

        rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(transform.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

        if (MaxStep > 0)
        {
            // wander > little minus reward
            AddReward(-1f / MaxStep);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardACtion = 0;
        int turnAction = 0;

        if (Input.GetKey(KeyCode.W))
        {
            forwardACtion = 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            turnAction = 1; // Left Turn
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turnAction = 2; // Right Turn
        }

        actionsOut.DiscreteActions.Array[0] = forwardACtion;
        actionsOut.DiscreteActions.Array[1] = turnAction;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Fish"))
        {
            EatFish(other.transform.GetComponent<Fish>());
        }

        else if (other.transform.CompareTag("Baby"))
        {
            RegurgitateFish();
        }
    }

    private void EatFish(Fish fishObject)
    {
        if (isFull) return;

        isFull = true;
        penguinArea.RemoveSpecificFish(fishObject);
        AddReward(1f);

        // If agent collide fish, add reward (1f)
    }

    private void RegurgitateFish()
    {
        if (!isFull) return;
        isFull = false;

        GameObject regurgitatedFish = Instantiate(regurgitatedFisnPrefab, baby.transform.position, Quaternion.identity, transform.parent);
        Destroy(regurgitatedFish, 4f);

        GameObject heart = Instantiate(heartPrefab, baby.transform.position + Vector3.up, Quaternion.identity, transform.parent);
        Destroy(heart, 4f);

        AddReward(1f);
        // if regurgitate fish to baby, add reward (1f)

        if (penguinArea.FishRemaining <= 0)
        {
            EndEpisode();
        }
    }
}
