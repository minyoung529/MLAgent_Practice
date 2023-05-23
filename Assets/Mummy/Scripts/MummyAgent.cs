using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MummyAgent : Agent
{
    private Rigidbody rigid;

    [SerializeField]
    private Transform target;

    private MummyStage mummyStage;

    private float moveSpeed = 5f;
    private bool isEnd = false;

    [SerializeField]
    private bool isDiscrete = false;

    [SerializeField]
    private TMP_Text rewardText;

    private void Update()
    {
        rewardText.text = GetCumulativeReward().ToString("0.00");
    }

    public override void Initialize()
    {
        rigid = GetComponent<Rigidbody>();
        mummyStage = transform.parent.GetComponent<MummyStage>();
    }

    public override void OnEpisodeBegin()
    {
        mummyStage?.StartEpisode();
        rigid.velocity = Vector3.zero;
        target.transform.localPosition = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
        transform.localPosition = new Vector3(Random.Range(-3.6f, 3.6f), 0.5f, Random.Range(-3.6f, 3.6f));
        isEnd = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(rigid.transform.localPosition);
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(target.position - rigid.transform.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isEnd) return;

        float forward = 0f;
        float right = 0f;

        if (isDiscrete)
        {
            DiscreteAction(actions, ref forward, ref right);
        }
        else
        {
            ContinousAction(actions, out forward, out right);
        }

        Vector3 forwardMove = transform.forward * forward;
        Vector3 rightMove = transform.right * right;

        rigid.MovePosition(transform.position + (forwardMove + rightMove) * moveSpeed * Time.fixedDeltaTime);
        AddReward(-1f/MaxStep);
    }

    #region ActionReceived
    private void ContinousAction(ActionBuffers actions, out float forward, out float right)
    {
        forward = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        right = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);
    }

    private void DiscreteAction(ActionBuffers actions, ref float forward, ref float right)
    {
        if (actions.DiscreteActions[0] == 1f)
            forward = 1f;
        else if (actions.DiscreteActions[0] == 2f)
            forward = -1f;

        if (actions.DiscreteActions[1] == 1f)
            right = 1f;
        else if (actions.DiscreteActions[1] == 2f)
            right = -1f;
    }
    #endregion

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (isDiscrete)
        {
            DiscreteHeuristic(actionsOut);
        }
        else
        {
            ContinousHeuristic(actionsOut);
        }
    }

    #region  Heuristic
    private static void ContinousHeuristic(ActionBuffers actionsOut)
    {
        float forwardAction = Input.GetAxis("Vertical");
        float rightAction = Input.GetAxis("Horizontal");

        actionsOut.ContinuousActions.Array[0] = forwardAction;
        actionsOut.ContinuousActions.Array[1] = rightAction;
    }

    private static void DiscreteHeuristic(ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        int rightAction = 0;

        if (Input.GetKey(KeyCode.W))
            forwardAction = 1;
        else if (Input.GetKey(KeyCode.S))
            forwardAction = 2;

        if (Input.GetKey(KeyCode.D))
            rightAction = 1; // Right Move
        else if (Input.GetKey(KeyCode.A))
            rightAction = 2; // Left Move

        actionsOut.DiscreteActions.Array[0] = forwardAction;
        actionsOut.DiscreteActions.Array[1] = rightAction;
    }
    #endregion

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("DEAD_ZONE"))
        {
            AddReward(-1f);
            mummyStage.Fail(EndEpisode);
            isEnd = true;
        }
        else if (other.transform.CompareTag("TARGET"))
        {
            AddReward(1f);
            mummyStage.Success(EndEpisode);
            isEnd = true;
        }
    }
}
