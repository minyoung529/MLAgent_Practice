using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MummyILAgent : Agent
{
    [SerializeField]
    private Renderer hint;

    [SerializeField]
    private Material[] materials;
    private int prev = -1;
    private float speed = 5f;

    private MummyILStage stage;

    private bool isStop = false;

    public override void Initialize()
    {
        stage = GetComponentInParent<MummyILStage>();
    }

    public override void OnEpisodeBegin()
    {
        prev = -1;
        hint.gameObject.name = "";

        int rand = Random.Range(0, materials.Length);

        while (rand == prev)
        {
            rand = Random.Range(0, materials.Length);
        }

        hint.material = materials[rand];
        hint.gameObject.name = materials[rand].name;
        prev = rand;
        transform.localPosition = Vector3.up * 0.05f;
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
        if (other.gameObject.name != "Black" && other.gameObject.name != "Green" &&
        other.gameObject.name != "Red" && other.gameObject.name != "Blue") return;

        if (other.gameObject == hint.gameObject)
        {
            AddReward(-0.06f);
            return;
        }

        if (other.gameObject.name == hint.gameObject.name)
        {
            AddReward(1f);
            stage.Success(EndEpisode);
        }
        else
        {
            AddReward(-1f);
            stage.Fail(EndEpisode);
        }
    }
}
